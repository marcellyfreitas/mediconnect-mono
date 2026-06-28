import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import { useRouter } from 'expo-router';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useCallback, useEffect, useState } from 'react';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { ActivityIndicator, FlatList, RefreshControl, Text } from 'react-native';
import CardCrud from '@/components/ui/CardCrud';
import { AdminMedicalExamsService } from '@/services/restrict/AdminMedicalExamsService';
import { useActionSheet } from '@expo/react-native-action-sheet';
import { useFocusEffect } from '@react-navigation/native';
import Toast from 'react-native-root-toast';
import { colors } from '@/utils/constants';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new AdminMedicalExamsService(client);

const MedicalExamsListScreen = () => {
	const router = useRouter();
	const [list, setList] = useState<any[]>([]);
	const [loading, setLoading] = useState<boolean>(true);
	const [error, setError] = useState<string | null>(null);
	const { showActionSheetWithOptions } = useActionSheet();

	const fetchExams = useCallback(async () => {
		try {
			setLoading(true);
			const response: any = await service.getAllMedicalExamsAsync({});
			setList(response.data.data);
			setError(null);
		} catch (err) {
			console.error('Erro ao buscar dados:', err);
			setList([]);
			setError('Erro ao carregar dados.');
		} finally {
			setLoading(false);
		}
	}, []);

	const onRefresh = useCallback(() => {
		fetchExams();
	}, [fetchExams]);

	useFocusEffect(
		useCallback(() => {
			fetchExams();
		}, [fetchExams])
	);

	function handleEdit(item: any) {
		router.push({
			pathname: '/adminzone/exames/exams',
			params: { ...item, editable: true },
		});
	}

	const deleteItem = useCallback(async (item: any) => {
		try {
			await service.putDeleteMedicalExamsAsync(item.id);
			Toast.show('Operação realizada com sucesso!', {
				duration: Toast.durations.SHORT,
				position: Toast.positions.BOTTOM,
				animation: true,
			});
			fetchExams();
		} catch (err) {
			console.error('Erro ao deletar item:', err);
			setError('Erro ao deletar item.');
		}
	}, [fetchExams]);

	const onPressDelete = useCallback((item: any) => {
		const options = ['Deletar', 'Cancelar'];
		const destructiveButtonIndex = 0;
		const cancelButtonIndex = 1;

		showActionSheetWithOptions({
			options,
			cancelButtonIndex,
			destructiveButtonIndex,
		}, (selected) => {
			if (selected === destructiveButtonIndex) {
				deleteItem(item);
			}
		});
	}, [deleteItem, showActionSheetWithOptions]);

	return (
		<ThemedView className="relative flex-1 w-full p-5">
			<ThemedView className="flex flex-col flex-1 w-full gap-4">
				{loading && <ActivityIndicator size="large" color="#007BFF" />}

				{error && <ThemedText className="text-center">{error}</ThemedText>}

				<FlatList
					data={list}
					keyExtractor={(item) => item.id.toString()}
					renderItem={({ item }) => (
						<CardCrud item={item} onDelete={onPressDelete} onEdit={handleEdit}>
							<Text>
								<Text className="font-bold text-lg text-slate-600">Exame: </Text>
								<Text className="text-lg font-semibold text-slate-600">{item.nomeExame}</Text>
							</Text>
							<Text>
								<Text className="font-bold text-lg text-slate-600">Descrição do Exame: </Text>
								<Text className="text-lg font-semibold text-slate-600">{item.descricaoExame}</Text>
							</Text>
						</CardCrud>
					)}
					contentContainerStyle={{ paddingBottom: 50, width: '100%', gap: 10 }}
					refreshControl={
						<RefreshControl
							refreshing={loading}
							onRefresh={onRefresh}
							colors={['#007BFF']}
							progressBackgroundColor="#FFFFFF"
						/>
					}
				/>
			</ThemedView>

			<Button
				onPress={() => router.push('/adminzone/exames/exams')}
				circular
				color="primary"
				className="!w-[50px] !h-[50px] absolute right-0 bottom-0 m-5 shadow"
			>
				<Ionicons size={25} name="add" color={colors.blue} />
			</Button>
		</ThemedView>
	);
};

export default MedicalExamsListScreen;