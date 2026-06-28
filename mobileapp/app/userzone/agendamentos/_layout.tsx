import { Stack } from 'expo-router';
import { colors } from '@/utils/constants';

export default function RootLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen name="index"
				options={{
					headerShown: true,
					headerTitle: 'Meus agendamentos',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>

			<Stack.Screen name="detalhes"
				options={{
					headerShown: true,
					headerTitle: 'Detalhes do agendamento',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>

			<Stack.Screen name="avaliacao"
				options={{
					headerShown: true,
					headerTitle: 'Avalie sua consulta',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>

			<Stack.Screen name="cancelamento"
				options={{
					headerShown: true,
					headerTitle: 'Confirmar cancelamento',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>
			<Stack.Screen name="pesquisar"
				options={{
					headerShown: true,
					headerTitle: 'Agendamento',
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
