import { useState } from 'react';
import { View, Text, KeyboardAvoidingView, Platform, ScrollView } from 'react-native';
import { useLocalSearchParams, useRouter } from 'expo-router';
import { ThemedView } from '@/components/ui/ThemedView';
import TextInput from '@/components/ui/TextInput';
import Button from '@/components/ui/Button';
import AuthHeader from '@/components/modules/auth/AuthHeader';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { AdminMedicalExamsService } from '@/services/restrict/AdminMedicalExamsService';
import { z } from 'zod';
import Toast from 'react-native-root-toast';

const formSchema = z.object({
	nomeExame: z.string().min(1, 'Campo obrigatório!!'),
	descricaoExame: z.string().min(1, 'Campo obrigatório!!'),
	setorResponsavel: z.string().min(1, 'Campo obrigatório!!'),
	tipoExame: z.string().min(1, 'Campo obrigatório!!'),
	prazoEntrega: z.string().min(1, 'Campo obrigatório!!'),
	requisitosPreparo: z.string().min(1, 'Campo obrigatório!!'),
	materialColeta: z.string().min(1, 'Campo obrigatório!!'),
});

type InnerFormData = z.infer<typeof formSchema>;

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new AdminMedicalExamsService(client);

export default function CreateExamsScreen() {
	const params = useLocalSearchParams();
	const { ...routeParams } = params;

	const { control, handleSubmit, formState: { errors } } = useForm<InnerFormData>({
		resolver: zodResolver(formSchema),
		defaultValues: {
			nomeExame: (routeParams.nomeExame ?? '') as string,
			descricaoExame: (routeParams.descricaoExame ?? '') as string,
			setorResponsavel: (routeParams.setorResponsavel ?? '') as string,
			tipoExame: (routeParams.tipoExame ?? '') as string,
			prazoEntrega: (routeParams.prazoEntrega ?? '') as string,
			requisitosPreparo: (routeParams.requisitosPreparo ?? '') as string,
			materialColeta: (routeParams.materialColeta ?? '') as string,
		},
	});

	const [inputHeight, setInputHeight] = useState(40);

	const router = useRouter();
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState('');
	const formatDateToIso = (date: string) => {
		const [day, month, year] = date.split('/');
		return `${year}-${month}-${day}`;
	};

	const onSubmit = async (data: InnerFormData) => {
		try {
			setLoading(true);

			const payload = {
				...data,
			};

			if (!routeParams.editable) {
				console.log('Payload enviado:', payload);
				await service.postMedicalExamsAsync(payload);
			}

			if (routeParams.editable) {
				await service.putUpdateMedicalExamsAsync(Number(routeParams.id), payload);
			}

			Toast.show('Operação realizada com sucesso!', {
				duration: Toast.durations.SHORT,
				position: Toast.positions.BOTTOM,
				animation: true,
			});

			router.back();
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
					<View className="flex flex-col w-full gap-5">
						<AuthHeader
							icon="map-outline"
							title={routeParams.editable ? 'Atualizar Exame' : 'Cadastrar Exame'}
							description={routeParams.editable ? 'Informe os dados solicitados para atualizar o exame:' : 'Informe os dados solicitados para cadastrar o exame:'}
						/>

						<View className="flex flex-col w-full gap-4">
							<Text className="font-bold text-white">Exame: </Text>
							<Controller
								control={control}
								name="nomeExame"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										returnKeyType="next"
										placeholder="Informe o nome do exame"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.nomeExame}
										errorMessage={errors.nomeExame?.message}
										multiline
										style={{
											paddingTop: 8,
											paddingBottom: 8,
											textAlignVertical: 'top',
										}}
									/>
								)}
							/>
							<Text className="font-bold text-white">Descrição do Exame: </Text>
							<Controller
								control={control}
								name="descricaoExame"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										returnKeyType="next"
										placeholder="Informe uma descrição do exame"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.descricaoExame}
										errorMessage={errors.descricaoExame?.message}
										multiline
										style={{
											paddingTop: 8,
											paddingBottom: 8,
											textAlignVertical: 'top',
										}}
									/>
								)}
							/>
							<Text className="font-bold text-white">Setor Responsável: </Text>
							<Controller
								control={control}
								name="setorResponsavel"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										returnKeyType="done"
										placeholder="Informe o setor responsável pelo exame"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.setorResponsavel}
										errorMessage={errors.setorResponsavel?.message}
										multiline
										style={{
											paddingTop: 8,
											paddingBottom: 8,
											textAlignVertical: 'top',
										}}
									/>
								)}
							/>
							<Text className="font-bold text-white">Tipo do Exame: </Text>
							<Controller
								control={control}
								name="tipoExame"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										returnKeyType="done"
										placeholder="Informe o tipo do exame"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.tipoExame}
										errorMessage={errors.tipoExame?.message}
										multiline
										style={{
											paddingTop: 8,
											paddingBottom: 8,
											textAlignVertical: 'top',
										}}
									/>
								)}
							/>
							<Text className="font-bold text-white">Prazo de entrega estimado do exame: </Text>
							<Controller
								control={control}
								name="prazoEntrega"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										returnKeyType="done"
										placeholder="Informe o prazo de entrega estimado"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.prazoEntrega}
										errorMessage={errors.prazoEntrega?.message}
										multiline
										style={{
											paddingTop: 8,
											paddingBottom: 8,
											textAlignVertical: 'top',
										}}
									/>
								)}
							/>
							<Text className="font-bold text-white">Requisitos de preparo para realização do exame: </Text>
							<Controller
								control={control}
								name="requisitosPreparo"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										returnKeyType="done"
										placeholder="Informe os requisitos de preparo para realização"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.requisitosPreparo}
										errorMessage={errors.requisitosPreparo?.message}
										multiline
										style={{
											paddingTop: 8,
											paddingBottom: 8,
											textAlignVertical: 'top',
										}}
									/>
								)}
							/>
							<Text className="font-bold text-white">Material de coleta do exame: </Text>
							<Controller
								control={control}
								name="materialColeta"
								render={({ field: { onChange, onBlur, value } }) => (
									<TextInput
										returnKeyType="done"
										placeholder="Informe o material de coleta do exame"
										value={value}
										onChangeText={onChange}
										onBlur={onBlur}
										disabled={loading}
										error={!!errors.materialColeta}
										errorMessage={errors.materialColeta?.message}
										multiline
										style={{
											paddingTop: 8,
											paddingBottom: 8,
											textAlignVertical: 'top',
										}}
									/>
								)}
							/>

							<Button
								onPress={handleSubmit(onSubmit)}
								color="primary"
								className="flex flex-row justify-center w-full gap-2"
								disabled={loading}
							>
								<Text className="font-bold text-secondary-500">
									{routeParams.editable ? 'Atualizar dados do exame' : 'Cadastrar exame'}
								</Text>
							</Button>
						</View>
					</View>
				</ThemedView>
			</ScrollView>
		</KeyboardAvoidingView>

	);
}