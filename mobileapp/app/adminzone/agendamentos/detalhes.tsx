import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import { HttpClient } from '@/services/restrict/HttpClient';
import { router, useLocalSearchParams } from 'expo-router';
import { ActivityIndicator, Text, TouchableOpacity, View } from 'react-native';
import { useCallback, useEffect, useState } from 'react';
import moment from 'moment';

import Button from '@/components/ui/Button';
import { Ionicons, MaterialCommunityIcons } from '@expo/vector-icons';
import Alert from '@/components/ui/Alert';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { AdminAppointmentsService } from '@/services/restrict/AdminAppointmentService';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new AdminAppointmentsService(client);

const AgendamentoDetalheScreen = () => {
	const params = useLocalSearchParams();
	const [loading, setLoading] = useState(false);
	const [agendamento, setAgendamento] = useState<any | null>(null);
	const [canAlter, setCanAlter] = useState<boolean>(false);
	const itemDate = moment(agendamento?.date);
	const today = moment();
	const diferencaEmDias = itemDate.diff(today, 'days');
	const diffAbs = Math.abs(diferencaEmDias);

	const fetchData = useCallback(async () => {
		setLoading(true);
		try {
			const id = params.id;
			const response = await service.getAppointmentByIdAsync(id as string);
			if (response.data.success) {
				setAgendamento(response.data.data);
				setCanAlter(diffAbs < 2 || ![
					'concluído',
					'cancelado',
					'concluída',
					'cancelada',
				].includes(String(agendamento?.status ?? '').toLowerCase()));
			}
		} catch (error) {
			console.error(error);
		} finally {
			setLoading(false);
		}
	}, [params.id, diffAbs, agendamento?.status]);

	useEffect(() => {
		fetchData();
	}, [fetchData]);

	function onPressEdit(item: any) {
		router.push({
			pathname: '/adminzone/agendamentos/pesquisar',
			params: { agendamentoId: item.id, userId: item.user.id },
		});
	}

	function onPressCancel(item: any) {
		router.push({
			pathname: '/adminzone/agendamentos/cancelamento',
			params: { agendamentoId: item.id },
		});
	}

	if (loading || !agendamento) {
		return (<ActivityIndicator size="small" />);
	}

	return (
		<ThemedView className="flex-1 gap-4 p-4">
			<View className="flex-1 gap-4">
				<ThemedText className="text-xl font-bold">
					Detalhes do Agendamento
				</ThemedText>


				<ThemedView className="gap-1">
					<ThemedText className="">Data: {moment(agendamento.date).format('DD/MM/YYYY [-] HH[h]')}</ThemedText>
					<ThemedText className="">Status: {agendamento.status}</ThemedText>
					<ThemedText className="">Protocolo: {agendamento.protocol}</ThemedText>
					{agendamento.notes && (
						<ThemedText className="">Observações: {agendamento.notes}</ThemedText>
					)}
				</ThemedView>

				<ThemedView className="gap-1">
					<ThemedText className="text-lg font-semibold">Profissional de saúde</ThemedText>
					<ThemedText>Nome: {agendamento.doctor.name}</ThemedText>
					<ThemedText>CRM: {agendamento.doctor.crm}</ThemedText>
					<ThemedText>Especialidade: {agendamento.doctor?.specialization?.name}</ThemedText>
				</ThemedView>


				<ThemedView className="gap-1">
					<ThemedText className="text-lg font-semibold">Unidade Médica</ThemedText>
					<ThemedText>Nome: {agendamento.medicalCenter.name}</ThemedText>
					<ThemedText>
						Endereço: {agendamento.medicalCenter.address.logradouro}, Nº {agendamento.medicalCenter.address.numero} - CEP {agendamento.medicalCenter.address.cep}
					</ThemedText>
				</ThemedView>

				<Alert
					title="Atenção"
					message="Os agendamentos podem ser cancelados até dois dias antes da data agendada."
					variant="warning"
				/>

			</View>

			<View className="flex flex-row gap-4">
				{canAlter && (
					<>
						<Button
							color="primary"
							className="flex-row items-center flex-1 gap-2"
							disabled={!canAlter}
							onPress={() => onPressEdit(agendamento)}
						>
							<MaterialCommunityIcons className="text-white" name="note-edit-outline" size={22} />
							<Text className="font-bold text-white">Editar</Text>
						</Button>
						<Button
							color="danger"
							className="flex-row items-center flex-1 gap-2"
							disabled={!canAlter}
							onPress={() => onPressCancel(agendamento)}
						>
							<Ionicons className="text-white" name="close-outline" size={22} />
							<Text className="font-bold text-white">Cancelar</Text>
						</Button>
					</>
				)}

			</View>
		</ThemedView>
	);
};

export default AgendamentoDetalheScreen;
