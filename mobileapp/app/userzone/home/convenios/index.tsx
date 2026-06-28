import React, { useCallback, useEffect, useState } from 'react';
import {
	View,
	Text,
	StyleSheet,
	FlatList,
	ActivityIndicator,
	RefreshControl,
} from 'react-native';
import { ThemedView } from '@/components/ui/ThemedView';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/UserAuthenticationContext';
import { MedicalAgreementService } from '@/services/public/MedicalAgreementService';
import Card from '../../../../components/ui/Card';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new MedicalAgreementService(client);

export default function ConveniosScreen() {
	const [conveniosList, setConveniosList] = useState([]);
	const [loading, setLoading] = useState(true);
	const [refreshing, setRefreshing] = useState(false);

	const fetchConvenios = useCallback(async () => {
		try {
			const { data: response } = await service.getAllAsync({});
			setConveniosList(response?.data || []);
		} catch (err) {
			console.error('Erro ao buscar convênios:', err);
			setConveniosList([]);
		} finally {
			setLoading(false);
		}
	}, []);

	const onRefresh = useCallback(async () => {
		setRefreshing(true);
		await fetchConvenios();
		setRefreshing(false);
	}, [fetchConvenios]);

	useEffect(() => {
		fetchConvenios();
	}, [fetchConvenios]);

	return (
		<ThemedView className="flex-1 p-4">
			{loading ? (
				<View style={styles.loadingContainer}>
					<ActivityIndicator size="large" color="#4a2d82" />
					<Text className="text-white">Carregando convênios...</Text>
				</View>
			) : (
				<FlatList
					data={conveniosList}
					keyExtractor={(item: any) => item.id.toString()}
					renderItem={({ item }: { item: any }) => (
						<Card>
							<View className="p-4">
								<Text className="font-semibold uppercase text-primary">{item.name}</Text>
							</View>
						</Card>
					)}
					contentContainerStyle={{ gap: 10 }}
					refreshControl={
						<RefreshControl
							refreshing={refreshing}
							onRefresh={onRefresh}
							colors={['#4a2d82']}
							tintColor="#4a2d82"
						/>
					}
				/>
			)}
		</ThemedView>
	);
}

const styles = StyleSheet.create({
	container: {
		flex: 1,
		backgroundColor: '#fff',
		padding: 20,
	},
	list: {
		paddingBottom: 20,
	},
	item: {
		flexDirection: 'row',
		alignItems: 'flex-start',
		marginBottom: 10,
	},
	bullet: {
		fontSize: 20,
		lineHeight: 22,
		marginRight: 10,
		color: '#4a2d82',
	},
	itemText: {
		fontSize: 16,
		color: '#333',
		flexShrink: 1,
	},
	loadingContainer: {
		flex: 1,
		justifyContent: 'center',
		alignItems: 'center',
	},
	loadingText: {
		marginTop: 10,
		color: '#4a2d82',
		fontSize: 16,
	},
});
