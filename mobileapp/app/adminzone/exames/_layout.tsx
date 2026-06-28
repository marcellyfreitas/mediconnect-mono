import { DrawerToggleButton } from '@react-navigation/drawer';
import { Stack } from 'expo-router';
import { colors } from '@/utils/constants';

export default function RootLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen
				name="index"
				options={{
					headerShown: true,
					title: 'Lista de exames',
					headerLeft: ({ tintColor }) => (<DrawerToggleButton tintColor={tintColor} />),
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>
			<Stack.Screen
				name="exams"
				options={{
					title: 'Formulário exames',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>
		</Stack>
	);
}