import { Stack } from 'expo-router';
import { DrawerToggleButton } from '@react-navigation/drawer';
import { colors } from '@/utils/constants';

export default function AddressLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen name="index" options={{
				headerShown: true,
				title: 'Unidades de saúde',
				headerStyle: {
					backgroundColor: colors.green,
				},
				headerTintColor: colors.blue,
				headerLeft: ({ tintColor }) => (<DrawerToggleButton tintColor={tintColor} />),
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
					headerShown:
						true,
					headerTitle: 'Dados do endereço',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>
			<Stack.Screen
				name="unidade"
				options={{
					headerShown: true,
					headerTitle: 'Dados da unidade',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>
		</Stack>
	);
}