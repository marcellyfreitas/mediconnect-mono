import { FlatList, RefreshControl, Text, TouchableOpacity, View } from 'react-native';
import { useCallback, useEffect, useState } from 'react';
import clsx from 'clsx';
import { Ionicons } from '@expo/vector-icons';
import { ThemedView } from '@/components/ui/ThemedView';
import { debounce } from '@/utils/debounce';
import TextInput from '@/components/ui/TextInput';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/UserAuthenticationContext';
import { router, useLocalSearchParams } from 'expo-router';
import { UserAppointmentsService } from '@/services/public/UserAppointmentsService';
import Card from '@/components/ui/Card';
import { isEmpty } from '@/utils/helpers';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UserAppointmentsService(client);

const AgendamentosScreen = () => {
	const routeParams = useLocalSearchParams();
	const [list, setList] = useState<any[]>([]);
	const [_, setLoadingSpecializations] = useState<boolean>(false);
	const [search, setSearch] = useState<string>('');
	const [specializationList, setSpecializationList] = useState<any[]>([]);
	const [selectedSpecialization, setSelectedSpecialization] = useState<number>(1);
	const [loading, setLoading] = useState<boolean>(false);
	const [agendamento, setAgendamento] = useState<any | null>(null);

	const fetchSpecializations = useCallback(async () => {
		setLoadingSpecializations(true);
		try {
			const response: any = await service.getAllSpecializationsAsync();
			if (response.data.success) {
				const arr = response.data.data;

				if (Array.isArray(arr)) {
					setSpecializationList(arr.reverse());
				}
			}
		} catch (error) {
			console.error('Erro!:', error);
		} finally {
			setLoadingSpecializations(false);
		}
	}, []);

	const fetchDoctors = async (searchTerm: string) => {
		setLoading(true);
		try {
			const response: any = await service.getAllMedicosAsync({
				search: searchTerm,
				especialidade: selectedSpecialization,
			});

			if (response.data.success) {
				setList(response.data.data);
			}
		} catch (error) {
			console.error('Erro!:', error);
		} finally {
			setLoading(false);
		}
	};

	// eslint-disable-next-line react-hooks/exhaustive-deps
	const debouncedFetchDoctors = useCallback(
		debounce(async (searchTerm: string) => {
			await fetchDoctors(searchTerm);
		}, 300),
		[selectedSpecialization],
	);

	useEffect(() => {
		fetchSpecializations();
	}, [fetchSpecializations]);


	useEffect(() => {
		debouncedFetchDoctors(search);
	}, [search, selectedSpecialization, debouncedFetchDoctors]);

	const fetchAgendamento = useCallback(async () => {
		if (isEmpty(routeParams.agendamentoId)) {
			return false;
		}

		setLoading(true);

		try {
			const id = routeParams.agendamentoId;
			const response = await service.getAppointmentByIdAsync(id as string);
			if (response.data.success) {
				setAgendamento(response.data.data);
			}
		} catch (error) {
			console.error('Erro!:', error);
		} finally {
			setLoading(false);
		}
	}, [routeParams.agendamentoId]);

	useEffect(() => {
		if (!isEmpty(routeParams.agendamentoId)) {
			fetchAgendamento();
		}
	}, [routeParams.agendamentoId, fetchAgendamento]);

	function handleDoctorPress({ doctorId, medicalCenterId }: any) {
		router.push({
			pathname: '/userzone/agendamentos/datepicker',
			params: { ...routeParams, doctorId, medicalCenterId },
		});
	}

	return (
		<ThemedView className="flex flex-col flex-1 gap-5 p-5">
			{
				agendamento && (
					<View className="flex-row gap-4 p-5 bg-gray-300 rounded">
						<Ionicons name="information-circle-outline" size={22} />

						<View>
							<Text className="font-bold">Em edição!</Text>
							<Text>Agendamento: {agendamento.protocol}</Text>
						</View>
					</View>
				)
			}

			<TextInput
				iconRight="search"
				placeholder="Pesquisar pelo nome"
				value={search}
				onChangeText={setSearch}
			/>

			<View>
				<FlatList
					horizontal
					showsHorizontalScrollIndicator={false}
					data={specializationList}
					keyExtractor={(item) => item.id.toString()}
					contentContainerStyle={{
						gap: 10,
						paddingHorizontal: 4,
						paddingVertical: 8,
						alignItems: 'center',
					}}
					style={{ height: 40, minHeight: 40 }}
					renderItem={({ item }) => (
						<TouchableOpacity
							className={clsx(
								'px-4 py-2 rounded-full min-h-[40px] items-center justify-center',
								selectedSpecialization === item.id
									? 'bg-primary-500'
									: 'bg-slate-300',
							)}
							onPress={() => setSelectedSpecialization(item.id)}
						>
							<Text
								className={clsx(
									selectedSpecialization === item.id ? 'text-white' : 'text-slate-600',
								)}
							>
								{item.name}
							</Text>
						</TouchableOpacity>
					)}
				/>
			</View>

			<View className="flex-1">
				<FlatList
					data={list}
					keyExtractor={(item, index) => (index + item.email.toString())}
					refreshing={loading}
					contentContainerStyle={{ padding: 0, gap: 10 }}
					ListEmptyComponent={
						<View className="flex items-center justify-center flex-1 py-10">
							<Ionicons name="search-outline" size={32} color="#6B7280" />
							<Text className="mt-2 text-gray-500">Nenhum médico encontrado</Text>
						</View>
					}
					renderItem={({ item }) => (
						<Card onPress={() => handleDoctorPress({
							doctorId: item.doctorId,
							medicalCenterId: item.medicalCenter.id,
						})}
						>
							<View className="flex gap-4 p-5 border rounded-lg border-slate-300">
								<View className="flex flex-row items-center gap-4">
									<View><Ionicons name="at" size={22} /></View>
									<View className="flex flex-row justify-between flex-1">
										<View>
											<Text className="font-bold leading-5">{item.name}</Text>
											<Text className="text-xs">{item.specialization.name}</Text>
										</View>
										<Text className="">5/5</Text>
									</View>
								</View>

								<View className="flex flex-row gap-4">
									<View><Ionicons name="location-outline" size={22} /></View>

									<View className="flex justify-between">
										<Text className="">{item.medicalCenter.name}</Text>
										<Text className="">{item.medicalCenter.address.logradouro}, {item.medicalCenter.address.numero}</Text>
									</View>
								</View>
							</View>
						</Card>
					)}
					refreshControl={
						<RefreshControl
							refreshing={loading}
							onRefresh={() => { }}
							colors={['#007BFF']}
							progressBackgroundColor="#FFFFFF"
						/>
					}
				/>
			</View>
		</ThemedView>
	);
};

export default AgendamentosScreen;