import { useCallback, useEffect, useState } from 'react';
import { ActivityIndicator, FlatList, RefreshControl, Text, View } from 'react-native';
import { router, useLocalSearchParams } from 'expo-router';
import moment from 'moment';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/UserAuthenticationContext';
import { UserMedicalExamsService } from '@/services/public/UserMedicalExamsService';
import { ThemedView } from '@/components/ui/ThemedView';
import { Ionicons } from '@expo/vector-icons';
import Card from '@/components/ui/Card';
import Button from '@/components/ui/Button';
import { colors } from '@/utils/constants';
import EmptyList from '@/components/ui/EmptyList';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UserMedicalExamsService(client);

const MedicalExamsScreen = () => {
	const params = useLocalSearchParams();
	const [loading, setLoading] = useState<boolean>(false);
	const [list, setList] = useState<any[]>([]);

	const fetchAppointments = useCallback(async () => {
		setLoading(true);
		try {
			const response: any = await service.getAllMedicalExamsAsync({});
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

	useEffect(() => {
		if (params.status === 'registered') {
			router.replace('/userzone/home/exames');
		}
	}, [params.status, fetchAppointments]);

	function handleItemPress(item?: any) {
		if (item?.id) {
			router.push({
				pathname: '/userzone/home/exames/detalhes',
				params: { id: item?.id },
			});
		}
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
									<Text className="text-lg font-bold">{item.nomeExame}</Text>
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
		</ThemedView>
	);
};

export default MedicalExamsScreen;