import { useCallback, useEffect, useState } from 'react';
import { ActivityIndicator, FlatList, RefreshControl, Text, View } from 'react-native';
import { router, useLocalSearchParams } from 'expo-router';
import moment from 'moment';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/UserAuthenticationContext';
import { UserAppointmentsService } from '@/services/public/UserAppointmentsService';
import { ThemedView } from '@/components/ui/ThemedView';
import { Ionicons } from '@expo/vector-icons';
import Card from '@/components/ui/Card';
import Button from '@/components/ui/Button';
import { colors } from '@/utils/constants';
import EmptyList from '@/components/ui/EmptyList';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UserAppointmentsService(client);

const AgendamentosScreen = () => {
	const params = useLocalSearchParams();
	const [loading, setLoading] = useState<boolean>(false);
	const [list, setList] = useState<any[]>([]);

	const fetchAppointments = useCallback(async () => {
		setLoading(true);
		try {
			const response: any = await service.getAllAppointmentAsync({});
			if (response.data.success) {
				const arr = response.data.data;

				if (Array.isArray(arr)) {
					setList(arr);
				}
			}
		} catch (error) {
			console.error('Erro!:', error);
		} finally {
			setLoading(false);
		}
	}, []);

	useEffect(() => {
		fetchAppointments();
	}, [fetchAppointments]);

	function handleItemPress(item?: any) {
		if (item?.id) {
			router.push({
				pathname: '/userzone/agendamentos/detalhes',
				params: { id: item?.id },
			});
		}
	}
	function onPressNew() {
		router.push('/userzone/agendamentos/pesquisar');
	}

	if (loading) {
		return <ActivityIndicator size="small" />;
	}

	return (
		<ThemedView className="relative flex-1 w-full p-5">
			<ThemedView className="flex flex-col flex-1 w-full gap-4">
				<FlatList
					data={list}
					keyExtractor={(_, index) => (index.toString())}
					refreshing={loading}
					contentContainerStyle={{ padding: 0, gap: 10 }}
					ListEmptyComponent={<EmptyList />}
					renderItem={({ item }) => (
						<Card>
							<View className="flex gap-4 p-5 border rounded-lg border-slate-300">
								<View className="flex gap-1">
									<Text className="font-bold uppercase">{item.doctor.name} - {item.doctor.specialization.name}</Text>
									<Text>Data - {moment(item.date).format('DD/MM/YYYY [às] H[h]')}</Text>
									<Text>Status - {item.status}</Text>
									<Text>Protocolo - {item.protocol}</Text>
								</View>


								<View className="flex flex-row gap-2">
									<Button
										color="primary"
										className="flex flex-row items-center flex-1 gap-2"
										onPress={() => handleItemPress(item)}
									>
										<Ionicons size={20} name="search-outline" className="text-secondary-500" />
										<Text className="font-bold text-secondary-500">Detalhes</Text>
									</Button>
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
			</ThemedView>

			<Button
				onPress={onPressNew}
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