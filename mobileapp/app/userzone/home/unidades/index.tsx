import React, { useEffect, useState, useCallback } from 'react';
import { View, Text, FlatList, ActivityIndicator, StyleSheet } from 'react-native';
import { HttpClient } from '@/services/restrict/HttpClient';
import { UnidadesService } from '@/services/public/UnidadesService';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/UserAuthenticationContext';
import { ThemedView } from '../../../../components/ui/ThemedView';

interface Unidade {
	id: number;
	name: string;
	address?: {
		logradouro: string;
		numero: string;
	};
}
const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UnidadesService(client);

const UnidadesScreen = () => {
	const [loading, setLoading] = useState(true);
	const [error, setError] = useState<string | null>(null);
	const [unidades, setUnidades] = useState<Unidade[]>([]);

	const fetchData = useCallback(async () => {
		setLoading(true);
		try {

			// Faz a requisição
			const response = await service.getAllAsync();

			if (response.data?.success) {
				setUnidades(response.data.data);
			} else {
				throw new Error(response.data?.message ?? 'Erro ao carregar unidades.');
			}
		} catch (err: any) {
			console.error('Erro:', err);
			setError(err.message || 'Erro desconhecido');
		} finally {
			setLoading(false);
		}
	}, []);

	useEffect(() => {
		fetchData();
	}, [fetchData]);

	if (loading) {
		return <ActivityIndicator size="large" />;
	}

	if (error) {
		return (
			<View style={styles.container}>
				<Text style={{ color: 'red' }}>Erro: {error}</Text>
			</View>
		);
	}

	return (
		<ThemedView style={styles.container}>
			<FlatList
				data={unidades}
				keyExtractor={(item) => item.id.toString()}
				renderItem={({ item }) => (
					<View style={styles.unitItem}>
						<Text style={styles.unitName}>{item.name}</Text>
						<Text style={styles.unitAddress}>
							{item.address?.logradouro}, {item.address?.numero}
						</Text>
					</View>
				)}
			/>
		</ThemedView>
	);
};

export default UnidadesScreen;

const styles = StyleSheet.create({
	container: {
		flex: 1,
		padding: 20,
	},
	unitItem: {
		marginBottom: 16,
		padding: 12,
		backgroundColor: '#f5f5f5',
		borderRadius: 8,
	},
	unitName: {
		fontWeight: 'bold',
		fontSize: 16,
	},
	unitAddress: {
		fontSize: 14,
		color: '#666',
	},
});
