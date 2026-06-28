import Ionicons from '@expo/vector-icons/Ionicons';
import { Text } from 'react-native';
import Button from './Button';
import { router } from 'expo-router';
import { useUserAuth } from '@/hooks/useUserAuth';

const UserLogoutButton = () => {
	const { logout } = useUserAuth();

	async function onPressLogout() {
		await logout();
		router.replace('/auth/userlogin');
	}
	return (
		<Button color="primary" className="flex flex-row w-full gap-2" onPress={onPressLogout}>
			<Ionicons size={22} name="log-out-outline" className="text-secondary-500" />
			<Text className="font-bold text-secondary-500">Logout</Text>
		</Button>
	);
};

export default UserLogoutButton;