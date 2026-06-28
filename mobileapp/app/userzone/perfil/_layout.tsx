import { Stack } from 'expo-router';
import { colors } from '@/utils/constants';


export default function RootLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen
				name="index"
				options={{
					headerShown: true,
					headerTitle: 'Meu perfil',
					headerStyle: {
						backgroundColor: colors.green,
					},
					headerTintColor: colors.blue,
				}}
			/>
			<Stack.Screen name="enderecos" options={{ headerShown: false }} />
			<Stack.Screen name="meusdados" options={{ headerShown: false }} />
			<Stack.Screen name="exclusao" options={{ headerShown: false }} />
		</Stack>
	);
}
