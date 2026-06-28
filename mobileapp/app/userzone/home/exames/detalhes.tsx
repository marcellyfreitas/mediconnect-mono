import { View, Text, ActivityIndicator, KeyboardAvoidingView, Platform, ScrollView } from 'react-native';
import { useLocalSearchParams } from 'expo-router';
import { useCallback, useEffect, useState } from 'react';
import { ThemedView } from '@/components/ui/ThemedView';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/UserAuthenticationContext';
import { UserMedicalExamsService } from '@/services/public/UserMedicalExamsService';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UserMedicalExamsService(client);

const UserMedicalExamsDetailScreen = () => {
	const params = useLocalSearchParams();
	const [loading, setLoading] = useState(false);
	const [exame, setExame] = useState<any | null>(null);

	const fetchData = useCallback(async () => {
		setLoading(true);
		try {
			const id = params.id;
			const response = await service.getMedicalExamsByIdAsync(id as string);
			if (response.data.success) {
				setExame(response.data.data);
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

	if (loading || !exame) {
		return <ActivityIndicator size="small" />;
	}

	return (
		<KeyboardAvoidingView
			behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
			keyboardVerticalOffset={Platform.OS === 'ios' ? 64 : 0}
			style={{ flex: 1 }}
		>
			<ScrollView keyboardShouldPersistTaps="handled" contentContainerStyle={{ flexGrow: 1 }}>
				<ThemedView className="flex-1 p-8 bg-gray">
					<View className="flex flex-col w-full gap-5">
						<View>
							<Text className="mb-4 text-xl font-bold text-white">{exame.nomeExame}</Text>
						</View>

						<View className="flex flex-col w-full gap-4">
							<View>
								<Text className="font-bold text-white">Descrição do exame:</Text>
								<Text className="text-white">{exame.descricaoExame}</Text>
							</View>

							<View>
								<Text className="font-bold text-white">Setor Responsável:</Text>
								<Text className="text-white">{exame.setorResponsavel}</Text>
							</View>

							<View>
								<Text className="font-bold text-white">Tipo do exame:</Text>
								<Text className="text-white">{exame.tipoExame}</Text>
							</View>

							<View>
								<Text className="font-bold text-white">Prazo de entrega estimado do exame:</Text>
								<Text className="text-white">{exame.prazoEntrega}</Text>
							</View>

							<View>
								<Text className="font-bold text-white">Requisitos de preparo:</Text>
								<Text className="text-white">{exame.requisitosPreparo}</Text>
							</View>

							<View>
								<Text className="font-bold text-white">Material de coleta:</Text>
								<Text className="text-white">{exame.materialColeta}</Text>
							</View>
						</View>
					</View>
				</ThemedView>
			</ScrollView>
		</KeyboardAvoidingView>
	);
};

export default UserMedicalExamsDetailScreen;
