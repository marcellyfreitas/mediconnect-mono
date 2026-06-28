import { useState } from 'react';
import { View, Text, KeyboardAvoidingView, Platform, ScrollView } from 'react-native';
import { useLocalSearchParams, useRouter } from 'expo-router';
import { ThemedView } from '@/components/ui/ThemedView';
import TextInput from '@/components/ui/TextInput';
import Button from '@/components/ui/Button';
import AuthHeader from '@/components/modules/auth/AuthHeader';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import Ionicons from '@expo/vector-icons/Ionicons';
import { colors } from '@/utils/constants';

const loginSchema = z.object({
	cep: z.string().min(1, 'Campo obrigatório!!'),
	logradouro: z.string().min(1, 'Campo obrigatório!!'),
	numero: z.string().min(0),
	complemento: z.string().min(0),
	bairro: z.string().min(1, 'Campo obrigatório!!'),
	cidade: z.string().min(1, 'Campo obrigatório!!'),
	estado: z.string().min(1, 'Campo obrigatório!!'),
});

type LoginFormData = z.infer<typeof loginSchema>;

export default function AddressScreen() {
	const router = useRouter();
	const params = useLocalSearchParams();
	const { ...routeParams } = params;

	const { control, handleSubmit, formState: { errors } } = useForm<LoginFormData>({
		resolver: zodResolver(loginSchema),
		defaultValues: {
			cep: (routeParams.cep ?? '') as string,
			logradouro: (routeParams.logradouro ?? '') as string,
			numero: (routeParams.numero) as string,
			complemento: (routeParams.complemento ?? '') as string,
			bairro: (routeParams.bairro ?? '') as string,
			cidade: ((routeParams.localidade ?? routeParams.cidade) ?? '') as string,
			estado: (routeParams.estado ?? '') as string,
		},
	});

	const [loading] = useState(false);

	const onSubmit = async (data: LoginFormData) => {
		router.push({
			pathname: '/adminzone/unidades/unidade',
			params: { ...routeParams, ...data },
		});
	};

	return (
		<KeyboardAvoidingView
			behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
			keyboardVerticalOffset={Platform.OS === 'ios' ? 64 : 0}
			style={{ flex: 1 }}
		>
			<ScrollView keyboardShouldPersistTaps="handled" contentContainerStyle={{ flexGrow: 1 }}>
				<ThemedView className="items-center justify-center flex-1 p-8 bg-gray">
					<View className="flex flex-col w-full gap-5">
						<AuthHeader
							icon="map-outline"
							title="Cadastrar endereço"
							description="Insira os dados solicitados abaixo para cadastrar seu endereço."
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
										disabled={true}
										error={!!errors.cep}
										errorMessage={errors.cep?.message}
									/>
								)}
							/>

							<Controller
								control={control}
								name="logradouro"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										returnKeyType="next"
										placeholder="Logradouro"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.logradouro}
										errorMessage={errors.logradouro?.message}
									/>
								)}
							/>

							<Controller
								control={control}
								name="numero"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										returnKeyType="next"
										placeholder="Número"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.numero}
										errorMessage={errors.numero?.message}
									/>
								)}
							/>

							<Controller
								control={control}
								name="complemento"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										returnKeyType="next"
										placeholder="Complemento"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.complemento}
										errorMessage={errors.complemento?.message}
									/>
								)}
							/>

							<Controller
								control={control}
								name="bairro"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										returnKeyType="next"
										placeholder="Bairro"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={false}
										error={!!errors.bairro}
										errorMessage={errors.bairro?.message}
									/>
								)}
							/>

							<View className="grid grid-cols-2 gap-4">
								<Controller
									control={control}
									name="cidade"
									render={({ field: { onChange, onBlur, value } }) => (
										<TextInput
											returnKeyType="next"
											placeholder="Cidade"
											value={value}
											onChangeText={onChange}
											onBlur={onBlur}
											disabled={true}
											error={!!errors.cidade}
											errorMessage={errors.cidade?.message}
										/>
									)}
								/>
								<Controller
									control={control}
									name="estado"
									render={({ field: { onChange, onBlur, value } }) => (
										<TextInput
											returnKeyType="next"
											placeholder="Estado"
											value={value}
											onChangeText={onChange}
											onBlur={onBlur}
											disabled={true}
											error={!!errors.estado}
											errorMessage={errors.estado?.message}
										/>
									)}
								/>
							</View>

							<Button
								onPress={handleSubmit(onSubmit)}
								color="primary"
								className="flex flex-row justify-center w-full gap-2"
								disabled={loading}
							>
								<Text className="font-bold text-secondary-500">
									Seguinte
								</Text>
								<Ionicons name="arrow-forward" color={colors.blue} />
							</Button>
						</View>
					</View>
				</ThemedView>
			</ScrollView>
		</KeyboardAvoidingView>

	);
}