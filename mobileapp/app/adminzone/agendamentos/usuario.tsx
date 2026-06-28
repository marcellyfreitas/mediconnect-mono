import { FlatList, RefreshControl, Text, View } from 'react-native';
import { useCallback, useEffect, useState } from 'react';
import { ThemedView } from '@/components/ui/ThemedView';
import TextInput from '@/components/ui/TextInput';
import EmptyList from '@/components/ui/EmptyList';
import Card from '@/components/ui/Card';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { AdminAppointmentsService } from '@/services/restrict/AdminAppointmentService';
import { debounce } from '@/utils/debounce';
import { router, useLocalSearchParams } from 'expo-router';
import { isEmpty } from '@/utils/helpers';
import { Ionicons } from '@expo/vector-icons';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new AdminAppointmentsService(client);

const UsuarioScreen = () => {
	const routeParams = useLocalSearchParams();
	const params = useLocalSearchParams();
	const [list, setlist] = useState<any[]>([]);
	const [loading, setLoading] = useState<boolean>(false);
	const [search, setSearch] = useState<string>('');
	const [agendamento, setAgendamento] = useState<any | null>(null);

	const fetchUsers = async (searchTerm = '') => {
		setLoading(true);

		try {
			const response = await service.getAllUsuariosAsync({ search: searchTerm });
			setlist(response.data.data);
		} catch (error) {
			console.error(error);
		} finally {
			setLoading(false);
		}
	};

	// eslint-disable-next-line react-hooks/exhaustive-deps
	const fetchDebouncedUsers = useCallback(
		debounce(async (searchTerm: string) => {
			await fetchUsers(searchTerm);
		}, 100),
		[],
	);

	useEffect(() => {
		fetchDebouncedUsers(search);
	}, [search, fetchDebouncedUsers]);

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

	function handleUserClick(user: any) {
		router.push({
			pathname: '/adminzone/agendamentos/pesquisar',
			params: { ...params, userId: user.id },
		});
	}

	return (
		<ThemedView className="flex-1 gap-4 p-4">
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
				placeholder="Pesquisar nome de usuário"
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
					<Card onPress={() => handleUserClick(item)}>
						<View className="flex gap-4 p-5 border rounded-lg border-slate-300">
							<Text>{item.name}</Text>
							<Text>{item.email}</Text>
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
		</ThemedView>
	);
};

export default UsuarioScreen;