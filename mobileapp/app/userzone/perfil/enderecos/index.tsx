import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import { useRouter } from 'expo-router';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useCallback, useEffect, useState } from 'react';
import { ActivityIndicator, FlatList, RefreshControl, Text } from 'react-native';
import CardCrud from '@/components/ui/CardCrud';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/UserAuthenticationContext';
import { useActionSheet } from '@expo/react-native-action-sheet';
import Toast from 'react-native-root-toast';
import { HttpClient } from '@/services/restrict/HttpClient';
import { UserAddressService } from '@/services/public/UserAddressService';
import { colors } from '@/utils/constants';
import { useUserAuth } from '@/hooks/useUserAuth';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UserAddressService(client);

export default function AddressListScreen() {
	const router = useRouter();
	const [list, setList] = useState<any[]>([]);
	const [loading, setLoading] = useState<boolean>(true);
	const [error, setError] = useState<string | null>(null);
	const { showActionSheetWithOptions } = useActionSheet();
	const { userData } = useUserAuth();

	const fetchAddresses = useCallback(async () => {
		try {
			setLoading(true);
			const response = await service.getAllFromUserAsync(userData?.id ?? 0);
			setList(response);
			setError(null);
		} catch (err) {
			console.error('Erro ao buscar endereços:', err);
			setList([]);
			setError('Erro ao carregar endereços.');
		} finally {
			setLoading(false);
		}
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [userData?.id]);

	const onRefresh = useCallback(() => {
		fetchAddresses();
	}, [fetchAddresses]);

	useEffect(() => {
		fetchAddresses();
	}, [fetchAddresses]);

	const deleteItem = useCallback(async (item: any) => {
		try {
			await service.deleteFromUserAsync(item.id);
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
			pathname: '/userzone/perfil/enderecos/address',
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
					keyExtractor={(item) => item?.id.toString()}
					renderItem={({ item }) => (
						<CardCrud item={item} onDelete={onPressDelete} onEdit={handleEdit}>
							<Text className="text-sm font-semibold text-slate-600">{item.cep} - {item.logradouro}, {item.numero}</Text>
							<Text className="text-sm uppercase text-slate-600">{item.cidade} - {item.estado}</Text>
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
				onPress={() => router.push('/userzone/perfil/enderecos/postalcode')}
				circular
				color="primary"
				className="!w-[50px] !h-[50px] absolute right-0 bottom-0 m-5 shadow"
				disabled={!!list.length}
			>
				<Ionicons size={25} name="add" color={colors.blue} />
			</Button>
		</ThemedView>
	);
};