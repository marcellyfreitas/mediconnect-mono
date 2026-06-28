import { Stack } from 'expo-router';
import { colors } from '@/utils/constants';
import { DrawerToggleButton } from '@react-navigation/drawer';

export default function RootLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen
				name="index"
				options={{
					headerShown: true,
					title: 'Agendamentos',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
					headerLeft: ({ tintColor }) => (<DrawerToggleButton tintColor={tintColor} />),
				}}
			/>

			<Stack.Screen name="detalhes"
				options={{
					headerShown: true,
					headerTitle: 'Detalhes',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>

			<Stack.Screen name="cancelamento"
				options={{
					headerShown: true,
					headerTitle: 'Cancelamento',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>

			<Stack.Screen name="usuario"
				options={{
					headerShown: true,
					headerTitle: 'Selecione um usuário',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>

			<Stack.Screen name="pesquisar"
				options={{
					headerShown: true,
					headerTitle: 'Profissionais de saúde',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>
			<Stack.Screen name="datepicker"
				options={{
					headerShown: true,
					headerTitle: 'Escolha uma data',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>

			<Stack.Screen name="timepicker"
				options={{
					headerShown: true,
					headerTitle: 'Escolha um horário',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>

			<Stack.Screen name="resume"
				options={{
					headerShown: true,
					headerTitle: 'Resumo',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>
		</Stack>
	);
}