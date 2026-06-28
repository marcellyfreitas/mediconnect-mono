import { ThemedView } from '@/components/ui/ThemedView';
import { useCallback, useEffect, useState } from 'react';
import { FlatList, RefreshControl, Text, View } from 'react-native';
import EmptyList from '@/components/ui/EmptyList';
import Card from '@/components/ui/Card';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { AdminAppointmentsService } from '@/services/restrict/AdminAppointmentService';
import { HttpClient } from '@/services/restrict/HttpClient';
import TextInput from '@/components/ui/TextInput';
import { debounce } from '@/utils/debounce';
import Button from '@/components/ui/Button';
import { Ionicons } from '@expo/vector-icons';
import { colors } from '@/utils/constants';
import moment from 'moment';
import { router, useLocalSearchParams } from 'expo-router';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new AdminAppointmentsService(client);

const AgendamentosScreen = () => {
	const routeParams = useLocalSearchParams();

	const [list, setlist] = useState<any[]>([]);
	const [loading, setloading] = useState<boolean>(false);
	const [search, setSearch] = useState<string>(routeParams.protocolo as string ?? '');

	const fetchAppointments = async (searchTerm = '') => {
		setloading(true);

		try {
			const response = await service.getAllAppointmentAsync({ search: searchTerm });
			setlist(response.data.data);
		} catch (error) {
			console.error(error);
		} finally {
			setloading(false);
		}
	};

	// eslint-disable-next-line react-hooks/exhaustive-deps
	const debouncedFetchAppointments = useCallback(
		debounce(async (searchTerm: string) => {
			await fetchAppointments(searchTerm);
		}, 100),
		[],
	);

	useEffect(() => {
		debouncedFetchAppointments(search);
	}, [search, debouncedFetchAppointments]);

	function handleItemPress(item?: any) {
		if (item?.id) {
			router.push({
				pathname: '/adminzone/agendamentos/detalhes',
				params: { id: item?.id },
			});
		}
	}

	return (
		<ThemedView className="relative flex-1 w-full p-5">
			<View className="flex flex-col flex-1 w-full gap-4">

				<TextInput
					iconRight="search"
					placeholder="Pesquisar protocolo"
					value={search}
					onChangeText={setSearch}
				/>

				<FlatList
					data={list}
					keyExtractor={(item, index) => (index.toString())}
					refreshing={loading}
					contentContainerStyle={{ padding: 0, gap: 10 }}
					ListEmptyComponent={<EmptyList />}
					renderItem={({ item, index }) => (
						<Card onPress={() => { }}>
							<View className="flex gap-2 p-5 border rounded-lg border-slate-300">
								<Text>Data: {moment(item.date).format('DD/MM/YYYY [-] H[h]')} { }</Text>
								<Text>Profissional: {item.doctor.name}</Text>
								<Text>Status: {item.status}</Text>
								<Text>Usuário: {item.user.name}</Text>

								<Button onPress={() => handleItemPress(item)} color="primary" className="flex-row items-center w-full gap-2">
									<Ionicons name="search" size={20} className="!text-secondary-500" />
									<Text className="font-bold text-secondary-500">Detalhes</Text>
								</Button>
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

			<Button
				onPress={() => router.push('/adminzone/agendamentos/usuario')}
				circular
				color="primary"
				className="!w-[50px] !h-[50px] absolute right-0 bottom-0 m-5 shadow"
			>
				<Ionicons size={25} name="add" color={colors.blue} />
			</Button>
		</ThemedView>
	);
};

export default AgendamentosScreen;