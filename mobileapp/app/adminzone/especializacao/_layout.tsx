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
					title: 'Especializações',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
					headerLeft: ({ tintColor }) => (<DrawerToggleButton tintColor={tintColor} />),
				}}
			/>
			<Stack.Screen
				name="Especialidade"
				options={{
					title: 'Formulário especialização',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>
		</Stack>
	);
}