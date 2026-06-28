import { Stack } from 'expo-router';
import { colors } from '@/utils/constants';

export default function AddressLayout() {

	return (
		<Stack initialRouteName="index">
			<Stack.Screen
				name="index"
				options={{
					headerShown: true,
					headerTitle: 'Meus endereços',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>
			<Stack.Screen
				name="postalcode"
				options={{
					headerShown: true,
					headerTitle: 'Buscar CEP',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>
			<Stack.Screen
				name="address"
				options={{
					headerShown: true,
					headerTitle: 'Cadastrar endereço',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>
		</Stack>
	);
}