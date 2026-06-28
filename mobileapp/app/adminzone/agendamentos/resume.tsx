import { ThemedView } from '@/components/ui/ThemedView';
import { router, useLocalSearchParams } from 'expo-router';
import { useCallback, useEffect, useState } from 'react';
import { HttpClient } from '@/services/restrict/HttpClient';
import { ActivityIndicator, Text, View } from 'react-native';
import { isEmpty } from '@/utils/helpers';
import Button from '@/components/ui/Button';
import { Ionicons } from '@expo/vector-icons';
import moment from 'moment';
import Toast from 'react-native-root-toast';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { AdminAppointmentsService } from '@/services/restrict/AdminAppointmentService';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new AdminAppointmentsService(client);

const HomeResumeScreen = () => {
	const [loading, setLoading] = useState<boolean>(false);
	const [doctor, setDoctor] = useState<any>(null);
	const [medicalCenter, setMedicalCenter] = useState<any>(null);
	const [selectedDate, setSelectedDate] = useState<string>('');
	const [formatedDate, setFormatedDate] = useState<string>('');
	const [agendamento, setAgendamento] = useState<any | null>(null);
	const [user, setUser] = useState<any | null>(null);


	const params = useLocalSearchParams();

	useEffect(() => {
		const { date, horario } = params;

		if (!isEmpty(date) && !isEmpty(horario)) {
			const formedDate = `${date}T${String(horario).split('-').shift()?.trim()}`;
			setSelectedDate(moment(formedDate, 'YYYY-MM-DDTH:mm').format('YYYY-MM-DDTHH:mm:ss'));
			setFormatedDate(moment(formedDate, 'YYYY-MM-DDTH:mm').format('DD/MM/YYYY [às] HH:mm'));
		}
	}, [selectedDate, formatedDate, params]);

	const fetchUser = useCallback(async () => {
		setLoading(true);

		try {
			const response: any = await service.getUserByIdAsync(params.userId as string);
			if (response.data.success) {
				const obj = response.data.data;

				if (!isEmpty(obj)) {
					setUser(response.data.data);
				}
			}
		} catch (error) {
			console.error('Erro!:', error);
		} finally {
			setLoading(false);
		}
	}, [params.userId]);

	useEffect(() => {
		fetchUser();
	}, [fetchUser]);

	const fetchDoctor = useCallback(async () => {
		setLoading(true);

		try {
			const response: any = await service.getDoctorByIdAsync(params.doctorId as string);
			if (response.data.success) {
				const obj = response.data.data;

				if (!isEmpty(obj)) {
					setDoctor(response.data.data);
				}
			}
		} catch (error) {
			console.error('Erro!:', error);
		} finally {
			setLoading(false);
		}
	}, [params.doctorId]);

	useEffect(() => {
		fetchDoctor();
	}, [fetchDoctor]);

	const fetchMedicalCenter = useCallback(async () => {
		setLoading(true);

		try {
			const response: any = await service.getMedicalCenterByIdAsync(params.medicalCenterId as string);
			if (response.data.success) {
				const obj = response.data.data;

				if (!isEmpty(obj)) {
					setMedicalCenter(response.data.data);
				}
			}
		} catch (error) {
			console.error('Erro!:', error);
		} finally {
			setLoading(false);
		}
	}, [params.medicalCenterId]);

	useEffect(() => {
		fetchMedicalCenter();
	}, [fetchMedicalCenter]);

	const fetchAgendamento = useCallback(async () => {
		if (isEmpty(params.agendamentoId)) {
			return false;
		}

		setLoading(true);

		try {
			const id = params.agendamentoId;
			const response = await service.getAppointmentByIdAsync(id as string);
			if (response.data.success) {
				setAgendamento(response.data.data);
			}
		} catch (error) {
			console.error('Erro!:', error);
		} finally {
			setLoading(false);
		}
	}, [params.agendamentoId]);

	useEffect(() => {
		fetchAgendamento();
	}, [fetchAgendamento]);

	if (loading) {
		return (<ActivityIndicator size="small" />);
	}

	const handleNext = async () => {
		setLoading(true);

		try {
			const payload = {
				'date': selectedDate,
				'status': 'Agendado',
				'notes': 'agendado pelo app',
				'doctorId': params.doctorId,
				'medicalCenterId': params.medicalCenterId,
				'userId': params.userId,
			};

			let response = null;

			if (agendamento?.id) {
				response = await service.putUpdateAppointmentAsync(agendamento.id, payload);
			} else {
				response = await service.postAppointmentAsync(payload);
			}

			Toast.show('Operação realizada com sucesso!', {
				duration: Toast.durations.SHORT,
				position: Toast.positions.BOTTOM,
				animation: true,
			});

			router.dismissAll();
			router.replace({
				pathname: '/adminzone/agendamentos',
				params: { status: 'registered', protocolo: response?.data?.data?.protocol ?? '' },
			});
		} catch (error) {
			console.error(error);
		} finally {
			setLoading(false);
		}
	};

	return (
		<ThemedView className="flex-1 gap-4 p-5">
			<View className="flex flex-1 gap-4">
				<Text className="font-semibold text-white">Você realizou a seleção do proficional:</Text>

				<View className="flex flex-row items-center gap-4">
					<View><Ionicons className="text-white" name="at" size={22} /></View>
					<View className="flex flex-row justify-between flex-1">
						<View>
							<Text className="font-bold leading-5 text-white">{doctor?.name}</Text>
							<Text className="text-xs text-white">{doctor?.specialization?.name}</Text>
						</View>
					</View>
				</View>

				<Text className="font-semibold text-white">Na unidade:</Text>

				<View className="flex flex-row gap-4">
					<View><Ionicons className="text-white" name="location-outline" size={22} /></View>

					<View className="flex justify-between">
						<Text className="text-white ">{medicalCenter?.name}</Text>
						<Text className="text-white ">{medicalCenter?.address?.logradouro}, {medicalCenter?.address?.numero}</Text>
					</View>
				</View>

				<Text className="font-semibold text-white">Para o usuário:</Text>

				<View className="flex flex-row gap-4">
					<View><Ionicons className="text-white" name="person-outline" size={22} /></View>

					<View className="flex justify-between">
						<Text className="text-white">{user?.name}</Text>
						<Text className="text-white">{user?.email}</Text>
					</View>
				</View>

				<Text className="font-semibold text-white">Na seguinte data:</Text>

				<View className="flex flex-row gap-4">
					<View><Ionicons className="text-white" name="calendar-clear-outline" size={22} /></View>

					<View className="flex justify-between">
						<Text className="text-white ">{formatedDate}</Text>
					</View>
				</View>

				{agendamento && (
					<View className="flex gap-4">
						<Text className="font-semibold text-white">Você está alterando o agendamento:</Text>

						<View className="flex justify-between">
							<Text className="text-white ">Protocolo: {agendamento.protocol}</Text>
						</View>
					</View>
				)}
			</View>
			<Button className="w-full" color="primary" onPress={handleNext}>
				<Text className="font-bold text-secondary-500">{!isEmpty(params.agendamentoId) ? 'Alterar agendamento' : 'Agendar'}</Text>
			</Button>
		</ThemedView>
	);
};

export default HomeResumeScreen;