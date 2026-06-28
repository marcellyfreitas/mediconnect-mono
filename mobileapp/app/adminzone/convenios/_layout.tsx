import { colors } from '@/utils/constants';
import { DrawerToggleButton } from '@react-navigation/drawer';
import { Stack } from 'expo-router';

export default function RootLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen
				name="index"
				options={{
					headerShown: true,
					title: 'Lista de Convênios',
					headerLeft: ({ tintColor }) => (<DrawerToggleButton tintColor={tintColor} />),
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>
		</Stack>
	);
}