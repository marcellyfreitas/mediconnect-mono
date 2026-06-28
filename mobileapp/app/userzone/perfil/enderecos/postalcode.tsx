import { useState } from 'react';
import { View, Text, KeyboardAvoidingView, Platform } from 'react-native';
import { router } from 'expo-router';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import TextInput from '@/components/ui/TextInput';
import AuthHeader from '@/components/modules/auth/AuthHeader';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { HttpClient } from '@/services/restrict/HttpClient';
import { z } from 'zod';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/UserAuthenticationContext';
import { UserAddressService } from '@/services/public/UserAddressService';
import { ScrollView } from 'react-native';

const cepSchema = z.object({
	cep: z
		.string()
		.min(8, 'O CEP deve ter pelo menos 8 dígitos')
		.max(9, 'O CEP pode ter no máximo 9 caracteres')
		.regex(/^\d{5}-?\d{3}$/, 'Formato de CEP inválido'),
});

type CepFormData = z.infer<typeof cepSchema>;

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UserAddressService(client);

export default function PostalCodeScreen() {
	const { control, handleSubmit, formState: { errors } } = useForm<CepFormData>({
		resolver: zodResolver(cepSchema),
		defaultValues: {
			cep: '',
			// cep: '30840-020',
		},
	});

	const [loading, setLoading] = useState(false);

	const onSubmit = async (data: CepFormData) => {
		try {
			const addressData = await service.search(data.cep);

			router.push({
				pathname: '/userzone/perfil/enderecos/address',
				params: { ...addressData.data, cep: data.cep },
			});
		} catch (error: any) {
			console.error(error);
		} finally {
			setLoading(false);
		}
	};

	return (
		<KeyboardAvoidingView
			behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
			keyboardVerticalOffset={Platform.OS === 'ios' ? 64 : 0}
			style={{ flex: 1 }}
		>
			<ScrollView keyboardShouldPersistTaps="handled" contentContainerStyle={{ flexGrow: 1 }}>
				<ThemedView className="items-center justify-center flex-1 p-8 bg-gray">
					<View className="flex flex-col w-full gap-4">
						<AuthHeader
							icon="map-outline"
							title="Buscar endereço"
							description="Insira seu CEP para que possamos tentar encontrar seu endereço."
						/>

						<View className="flex flex-col w-full gap-4">
							<Controller
								control={control}
								name="cep"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										returnKeyType="next"
										placeholder="CEP"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.cep}
										errorMessage={errors.cep?.message}
									/>
								)}
							/>

							<Button
								onPress={handleSubmit(onSubmit)}
								color="primary"
								className="w-full"
								disabled={loading}
							>
								<View className="flex flex-row items-center gap-2">
									<Text className="font-bold text-secondary">Endereço</Text>
									<Ionicons name="arrow-forward" color="#ffffff" />
								</View>
							</Button>
						</View>
					</View>
				</ThemedView>
			</ScrollView>
		</KeyboardAvoidingView>

	);
}