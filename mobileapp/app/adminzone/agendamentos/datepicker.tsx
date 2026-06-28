import { useCallback, useEffect, useState } from 'react';
import { ActivityIndicator, Text, TouchableOpacity, View } from 'react-native';
import { Calendar, DateData, LocaleConfig } from 'react-native-calendars';
import { router, useLocalSearchParams } from 'expo-router';
import { datepickerStyles as styles } from '@/styles/home';
import { ThemedView } from '@/components/ui/ThemedView';
import { Feather, Ionicons } from '@expo/vector-icons';
import { ptBR } from '@/utils/calendar';
import Button from '@/components/ui/Button';
import { isEmpty } from '@/utils/helpers';
import { colors } from '@/utils/constants';
import moment from 'moment';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { AdminAppointmentsService } from '@/services/restrict/AdminAppointmentService';


LocaleConfig.locales['pt-br'] = ptBR;
LocaleConfig.defaultLocale = 'pt-br';
const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new AdminAppointmentsService(client);

const AgendamentoDatePicker = () => {
	const [day, setDay] = useState<DateData>();
	const routeParams = useLocalSearchParams();
	const [loading, setLoading] = useState<boolean>(false);
	const [agendamento, setAgendamento] = useState<any | null>(null);

	const fetchAgendamento = useCallback(async () => {
		if (isEmpty(routeParams.agendamentoId)) {
			return false;
		}

		setLoading(true);

		try {
			const id = routeParams.agendamentoId;
			const response = await service.getAppointmentByIdAsync(id as string);
			if (response.data.success) {
				setAgendamento(response.data.data);
			}
		} catch (error) {
			console.error('Erro!:', error);
		} finally {
			setLoading(false);
		}
	}, [routeParams.agendamentoId]);

	useEffect(() => {
		if (!isEmpty(routeParams.agendamentoId)) {
			fetchAgendamento();
		}
	}, [routeParams.agendamentoId, fetchAgendamento]);

	const isHolyday = (date: DateData) => {
		const today = moment();
		const givenDate = moment(new Date(date.year, date.month - 1, date.day));
		const dayOfWeek = new Date(date.year, date.month - 1, date.day).getDay();
		return dayOfWeek === 0 || dayOfWeek === 6 || givenDate.isBefore(today);
	};

	function handleNext() {
		if (!isEmpty(day?.dateString)) {
			router.push({
				pathname: '/adminzone/agendamentos/timepicker',
				params: { ...routeParams, date: day?.dateString },
			});
		}
	}

	if (loading) {
		return (<ActivityIndicator size={'small'} />);
	}

	return (
		<ThemedView className="flex-1 p-5">
			<View className="flex-1">
				{
					agendamento && (
						<View className="flex-row gap-4 p-5 bg-gray-300 rounded">
							<Ionicons name="information-circle-outline" size={22} />

							<View>
								<Text className="font-bold">Em edição!</Text>
								<Text>Agendamento: {agendamento.protocol}</Text>
							</View>
						</View>
					)
				}

				<Calendar
					style={styles.calendar}
					renderArrow={(direction: 'right' | 'left') => (
						<Feather size={24} color={colors.gray} name={`chevron-${direction}`} />
					)}
					headerStyle={{
						borderBottomWidth: 0.5,
						borderBottomColor: '#DEDEDE',
						paddingBottom: 10,
						marginBottom: 10,
					}}
					theme={{
						calendarBackground: 'transparent',
						textMonthFontSize: 24,
						monthTextColor: colors.gray,
						arrowStyle: {
							margin: 0,
							padding: 0,
						},
					}}
					minDate={new Date().toDateString()}
					hideExtraDays
					onDayPress={setDay}
					markedDates={
						day && {
							[day.dateString]: { selected: true },
						}
					}
					dayComponent={({ date, state }: any) => {
						const disabled = isHolyday(date);

						return (
							<TouchableOpacity
								style={[
									styles.day,
									date.dateString === day?.dateString && styles.daySelected,
									disabled && styles.disabledDay,
								]}
								onPress={() => setDay(date)}
								disabled={disabled}
							>
								<Text
									style={[
										state === 'today' ? styles.today : styles.dayText,
									]}
								>
									{date.day}
								</Text>
							</TouchableOpacity>
						);
					}}
				/>
			</View>
			<Button onPress={handleNext} color="primary" className="w-full">
				<View className="flex flex-row items-center gap-2">
					<Text className="font-semibold text-secondary-500">Próximo</Text>
					<Ionicons name="arrow-forward-outline" className="text-secondary-500" size={18} />
				</View>
			</Button>
		</ThemedView>
	);
};

export default AgendamentoDatePicker;