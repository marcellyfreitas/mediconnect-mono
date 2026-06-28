import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import { useRouter } from 'expo-router';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useCallback, useEffect, useState } from 'react';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { ActivityIndicator, FlatList, RefreshControl, Text } from 'react-native';
import { UnitService } from '@/services/restrict/UnitService';
import CardCrud from '@/components/ui/CardCrud';
import Toast from 'react-native-root-toast';
import { useActionSheet } from '@expo/react-native-action-sheet';
import { colors } from '@/utils/constants';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UnitService(client);

const UnidadesScreen = () => {
	const router = useRouter();
	const [list, setList] = useState<any[]>([]);
	const [loading, setLoading] = useState<boolean>(true);
	const [error, setError] = useState<string | null>(null);
	const { showActionSheetWithOptions } = useActionSheet();

	const fetchAddresses = useCallback(async () => {
		try {
			setLoading(true);
			const { data: response } = await service.getAllFromAdminAsync();
			setList(response.data);
			setError(null);
		} catch (err) {
			console.error('Erro ao buscar endereços:', err);
			setList([]);
			setError('Erro ao carregar endereços.');
		} finally {
			setLoading(false);
		}

		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, []);

	const onRefresh = useCallback(() => {
		fetchAddresses();
	}, [fetchAddresses]);

	useEffect(() => {
		fetchAddresses();
	}, [fetchAddresses]);

	const deleteItem = useCallback(async (item: any) => {
		try {
			await service.deleteFromAdminAsync(item.id);
			Toast.show('Operação realizada com sucesso!', {
				duration: Toast.durations.SHORT,
				position: Toast.positions.BOTTOM,
				animation: true,
			});
			fetchAddresses();
		} catch (err) {
			console.error('Erro ao deletar item:', err);
			setError('Erro ao deletar item.');
		}
	}, [fetchAddresses]);

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

	function handleEdit(item: any) {
		const { address, ...rest } = item;
		const addressData = { ...address, ...rest, editable: true };

		router.push({
			pathname: '/adminzone/unidades/address',
			params: { ...addressData },
		});
	}

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
							<Text className="text-lg font-semibold uppercase text-slate-600">{item.name}</Text>
							<Text className="text-sm font-semibold text-slate-600">{item.address.cep} - {item.address.logradouro}, {item.address.numero}</Text>
							<Text className="text-sm uppercase text-slate-600">{item.address.cidade} - {item.address.estado}</Text>
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
				onPress={() => router.push('/adminzone/unidades/postalcode')}
				circular
				color="primary"
				className="!w-[50px] !h-[50px] absolute right-0 bottom-0 m-5 shadow"
			>
				<Ionicons size={25} name="add" color={colors.blue} />
			</Button>
		</ThemedView>
	);
};

export default UnidadesScreen;
