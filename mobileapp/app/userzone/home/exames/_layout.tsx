import { Stack } from 'expo-router';
import { colors } from '@/utils/constants';

export default function RootLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen name="index"
				options={{
					headerShown: true,
					headerTitle: 'Exames ofertados',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
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
		</Stack>
	);
}
