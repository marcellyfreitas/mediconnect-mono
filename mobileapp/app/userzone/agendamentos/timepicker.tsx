import { ActivityIndicator, FlatList, Text, TouchableOpacity, View } from 'react-native';
import { router, useLocalSearchParams } from 'expo-router';
import { useCallback, useEffect, useState } from 'react';
import clsx from 'clsx';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import { Ionicons } from '@expo/vector-icons';
import { isEmpty } from '@/utils/helpers';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/UserAuthenticationContext';
import { UserAppointmentsService } from '@/services/public/UserAppointmentsService';
import { appointmentDefaultHours } from '@/utils/configs';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UserAppointmentsService(client);

const AgendamentosTimePicker = () => {
	const [horarios] = useState<string[]>(appointmentDefaultHours);
	const [horario, setHorario] = useState<string>('');
	const [loading, setLoading] = useState<boolean>(false);
	const [_, setReservedHoursList] = useState<string[]>([]);
	const routeParams = useLocalSearchParams();
	const [agendamento, setAgendamento] = useState<any | null>(null);

	const params = useLocalSearchParams();

	const fetchReservedHoursList = useCallback(async () => {
		setLoading(true);
		try {
			const response: any = await service.getReservedHoursAsync(params.date as string);
			if (response.data.success) {
				const arr = response.data.data;

				if (Array.isArray(arr)) {
					setReservedHoursList(arr.reverse());
				}
			}
		} catch (error) {
			console.error('Erro!:', error);
		} finally {
			setLoading(false);
		}
	}, [params.date]);

	useEffect(() => {
		fetchReservedHoursList();
	}, [fetchReservedHoursList]);

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

	function handleNext() {
		if (!isEmpty(horario)) {
			router.push({
				pathname: '/userzone/agendamentos/resume',
				params: { ...params, horario },
			});
		}
	}

	if (loading) {
		return (<ActivityIndicator size="small" />);
	}

	return (
		<ThemedView className="flex-1 gap-5 p-5">
			<View className="flex-1 gap-4">
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

				<FlatList
					data={horarios}
					keyExtractor={(item, index) => index.toString()}
					contentContainerStyle={{ padding: 0, gap: 10 }}
					ListEmptyComponent={
						<View className="flex items-center justify-center flex-1 py-10">
							<Ionicons name="search-outline" size={32} color="#6B7280" />
							<Text className="mt-2 text-gray-500">Nenhum horário disponível encontrado</Text>
						</View>
					}
					renderItem={({ item }) => (
						<TouchableOpacity
							className={clsx(
								'flex-row items-center justify-between p-5 border rounded-lg border-slate-300',
								horario === item
									? 'bg-primary-500'
									: 'bg-slate-100',
							)}
							onPress={() => setHorario(item)}
							disabled={false}
						>
							<Text className={clsx('text-slate-500', horario === item
								? 'text-white'
								: 'text-slate-500')}
							>
								{item}
							</Text>
							<Ionicons name="time-outline" size={22} className={clsx('text-slate-500', horario === item
								? 'text-white'
								: 'text-slate-500')}
							/>
						</TouchableOpacity>
					)}
				/>
			</View>

			<Button className="w-full" color="primary" onPress={handleNext}>
				<Text className="font-bold text-secondary-500">Resumo</Text>
			</Button>
		</ThemedView>
	);
};

export default AgendamentosTimePicker;