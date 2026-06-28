import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import { Image, View } from 'react-native';

interface AuthHeaderProps {
	icon: string,
	title: string,
	description: string,
}

const AuthHeader = ({ title, description, icon }: AuthHeaderProps) => {
	return (
		<ThemedView>
			<View className="flex flex-row justify-center w-full h-[200px]">
				<Image
					source={require('@/assets/images/logo_white.png')}
					style={{
						width: '50%',
						height: 200,
						resizeMode: 'contain',
					}}
				/>
			</View>
			<View className="flex flex-row items-center w-full gap-2">
				<ThemedText className="text-2xl">{title}</ThemedText>
			</View>

			<View className="w-full">
				<ThemedText>{description}</ThemedText>
			</View>
		</ThemedView>
	);
};

export default AuthHeader;