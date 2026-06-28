import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import Toast from 'react-native-root-toast';
import { useCallback, useEffect, useState } from 'react';
import { router, useLocalSearchParams } from 'expo-router';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/UserAuthenticationContext';
import { UserAppointmentsService } from '@/services/public/UserAppointmentsService';
import moment from 'moment';
import { ActivityIndicator } from 'react-native';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UserAppointmentsService(client);

const CancelamentoScreen = () => {
	const params = useLocalSearchParams();
	const [loading, setLoading] = useState(false);
	const [agendamento, setAgendamento] = useState<any | null>(null);


	const fetchData = useCallback(async () => {
		setLoading(true);
		try {
			const id = params.agendamentoId;
			const response = await service.getAppointmentByIdAsync(id as string);
			if (response.data.success) {
				setAgendamento(response.data.data);
			}
		} catch (error) {
			console.error(error);
		} finally {
			setLoading(false);
		}
	}, [params.id]);

	useEffect(() => {
		fetchData();
	}, [fetchData]);

	const handleCancelar = useCallback(async () => {
		try {
			const id = params.agendamentoId;
			await service.putCancelAppointmentAsync(id as string);

			Toast.show('Operação realizada com sucesso!', {
				duration: Toast.durations.SHORT,
				position: Toast.positions.BOTTOM,
				animation: true,
			});
			router.dismissAll();
			router.replace('/userzone/agendamentos');
		} catch (err) {
			console.error('Erro ao deletar item:', err);
		}
	}, [params.agendamentoId]);

	if (loading || !agendamento) {
		return (<ActivityIndicator size="small" />);
	}

	return (
		<ThemedView className="flex-1 p-4">
			<ThemedView className="flex-1">
				<ThemedText className="mb-4 text-lg font-semibold">
					Tem certeza que deseja cancelar este agendamento?
				</ThemedText>
				<ThemedText className="mb-6 text-sm text-gray-300">
					Essa ação não poderá ser desfeita e o horário ficará disponível para outros usuários.
				</ThemedText>

				<ThemedView className="gap-1">
					<ThemedText className="">Data: {moment(agendamento.date).format('DD/MM/YYYY [-] HH[h]')}</ThemedText>
					<ThemedText className="">Status: {agendamento.status}</ThemedText>
					<ThemedText className="">Protocolo: {agendamento.protocol}</ThemedText>
					{agendamento.notes && (
						<ThemedText className="">Observações: {agendamento.notes}</ThemedText>
					)}
				</ThemedView>
			</ThemedView>
			<Button
				title="Confirmar Cancelamento"
				color="danger"
				onPress={handleCancelar}
				className="w-full mt-2"
			/>
		</ThemedView>
	);
};

export default CancelamentoScreen;
