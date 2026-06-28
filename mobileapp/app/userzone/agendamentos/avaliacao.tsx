import {
  TouchableOpacity,
  TextInput,
  StyleSheet,
  Text,
  ActivityIndicator,
  View
} from 'react-native';
import { colors } from '@/utils/constants';
import { useEffect, useState, useCallback } from 'react';
import { useLocalSearchParams, useFocusEffect } from 'expo-router';
import { AirbnbRating } from 'react-native-ratings';
import Toast from 'react-native-root-toast';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/UserAuthenticationContext';
import { HttpClient } from '@/services/restrict/HttpClient';
import { AppointmentRatingsService } from '@/services/public/AppointmentRatingsService';
import { useUserAuth } from '@/hooks/useUserAuth';
import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import moment from 'moment';
import { number } from 'zod';

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const ratingsService = new AppointmentRatingsService(client);

interface EvaluationState {
  rating: number;
  comment: string;
  evaluationId: number | null;
  loading: boolean;
  initialLoading: boolean;
}

const MAX_COMMENT_LENGTH = 500;

export default function AgendamentoAvaliacaoScreen() {
  const [state, setState] = useState<EvaluationState>({
    rating: 0,
    comment: '',
    evaluationId: null,
    loading: false,
    initialLoading: true
  });

  const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
  const [appointmentData, setAppointmentData] = useState<any>(null);

  const { appointmentId } = useLocalSearchParams();
  const { userData } = useUserAuth();
  const userId = userData?.id ? Number(userData.id) : null;
  const parsedAppointmentId = Number(appointmentId);

  const showToast = (message: string, isError = false) => {
    Toast.show(message, {
      duration: Toast.durations.SHORT,
      position: Toast.positions.BOTTOM,
      backgroundColor: isError ? '#FF0000' : colors.green,
      textColor: colors.white,
    });
  };

  const loadData = useCallback(async () => {
    if (!parsedAppointmentId) return;

    try {
      setState(prev => ({ ...prev, loading: true }));

      const response = await ratingsService.getAppointmentDetailsAsync(parsedAppointmentId);
      
      if (response.data?.success) {
        const rating = response.data.data?.rating;
        setState({
          rating: rating?.rating || 0,
          comment: rating?.comment || '',
          evaluationId: rating?.id || null,
          loading: false,
          initialLoading: false,
        });
      }
    } catch (error: any) {
      console.error('Erro ao carregar avaliação:', error.response?.data || error.message);
      showToast('Falha ao carregar avaliação', true);
      setState(prev => ({ ...prev, loading: false, initialLoading: false }));
    }
  }, [parsedAppointmentId]);

  const loadAppointmentDetails = async () => {
    try {
      const response = await ratingsService.getAppointmentDetailsAsync(parsedAppointmentId);
      if (response.data?.success) {
        setAppointmentData(response.data.data);
      }
    } catch (error) {
      console.error('Erro ao carregar detalhes:', error);
    }
  };

  useFocusEffect(useCallback(() => { loadData(); }, [loadData]));
  useEffect(() => { if (showDeleteConfirmation) loadAppointmentDetails(); }, [showDeleteConfirmation]);

  const handleSubmit = async () => {
    if (state.rating === 0) {
      showToast('Selecione uma nota entre 1 e 5 estrelas.', true);
      return;
    }

    try {
      setState(prev => ({ ...prev, loading: true }));

      const payload = {
        rating: state.rating,
        comment: state.comment,
        appointmentId: parsedAppointmentId,
        userId: Number(userId)
      };

      const response = state.evaluationId
        ? await ratingsService.putEvaluationAsync(state.evaluationId, payload)
        : await ratingsService.postEvaluationAsync(payload);

      if (response.data?.success) {
        setState(prev => ({
          ...prev,
          evaluationId: response.data.data?.id || prev.evaluationId,
          loading: false,
        }));

        showToast(state.evaluationId ? 'Avaliação atualizada!' : 'Avaliação enviada!');
        await loadData();
      }
    } catch (error: any) {
      console.error('Erro ao salvar avaliação:', error.response?.data || error.message);
      showToast('Falha ao enviar avaliação', true);
    } finally {
      setState(prev => ({ ...prev, loading: false }));
    }
  };

  const handleDeleteEvaluation = async () => {
    if (!state.evaluationId) {
      showToast('Nenhuma avaliação para excluir.', true);
      return;
    }

    try {
      setState(prev => ({ ...prev, loading: true }));
      setShowDeleteConfirmation(false);

      const response = await ratingsService.deleteEvaluationAsync(state.evaluationId);

      if ([200, 204].includes(response.status)) {
        showToast('Avaliação excluída!');
        setState({
          rating: 0,
          comment: '',
          evaluationId: null,
          loading: false,
          initialLoading: false
        });
        await loadData();
      }
    } catch (error: any) {
      console.error('Erro ao excluir:', error.response?.data || error.message);
      showToast('Falha ao excluir avaliação', true);
    } finally {
      setState(prev => ({ ...prev, loading: false }));
    }
  };

  if (showDeleteConfirmation) {
    return (
      <ThemedView className="flex-1 p-4" style={styles.container}>
        <ThemedView className="flex-1">
          <ThemedText className="mb-4 text-lg font-semibold" style={{ color: colors.white }}>
            Tem certeza que deseja excluir esta avaliação?
          </ThemedText>
          <ThemedText className="mb-6 text-sm" style={{ color: colors.white }}>
            Essa ação não poderá ser desfeita.
          </ThemedText>

          {appointmentData && (
            <ThemedView className="gap-1">
              <ThemedText style={{ color: colors.white }}>
                Data: {moment(appointmentData.date).format('DD/MM/YYYY [-] HH[h]')}
              </ThemedText>
              <ThemedText style={{ color: colors.white }}>
                Status: {appointmentData.status}
              </ThemedText>
              {appointmentData.protocol && (
                <ThemedText style={{ color: colors.white }}>
                  Protocolo: {appointmentData.protocol}
                </ThemedText>
              )}
            </ThemedView>
          )}
        </ThemedView>
        
        <Button
          title="Confirmar Exclusão"
          color="danger"
          onPress={handleDeleteEvaluation}
          className="w-full mt-2"
        />
        
        <Button
          title="Cancelar"
          onPress={() => setShowDeleteConfirmation(false)}
          className="w-full mt-2"
          style={{ backgroundColor: colors.blue }}
        />
      </ThemedView>
    );
  }

  if (state.initialLoading) {
    return (
      <View style={[styles.container, styles.loadingContainer]}>
        <ActivityIndicator size="large" color={colors.white} />
      </View>
    );
  }

 /* const ratingLabels = {
    0: 'Selecione sua avaliação',
    1: 'Ruim',
    2: 'Regular', 
    3: 'Bom',
    4: 'Muito bom',
    5: 'Excelente'
  };
 */
  return (
    <View style={styles.container}>
      <View style={styles.content}>
        <View style={styles.ratingContainer}>
          <AirbnbRating
            key={`rating-${state.evaluationId}-${state.rating}`}
            count={5}
            defaultRating={state.rating}
            selectedColor={colors.green}
            reviewColor={colors.green}
            size={32}
            isDisabled={state.loading}
            showRating={false}
            onFinishRating={(rating) => setState(prev => ({ ...prev, rating }))}
          />

          <Text style={styles.ratingText}>
            {/*ratingLabels[state.rating as keyof typeof ratingLabels]*/}
          </Text>
        </View>

        <Text style={styles.label}>Sugestões: (opcional)</Text>
        <View style={styles.inputContainer}>
          <TextInput
            placeholder="Informe sua sugestão sobre o agendamento ou sobre a sua experiência."
            placeholderTextColor={colors.grayHard}
            style={styles.input}
            multiline
            numberOfLines={4}
            maxLength={MAX_COMMENT_LENGTH}
            value={state.comment}
            onChangeText={(text) => setState(prev => ({ ...prev, comment: text }))}
            editable={!state.loading}
          />
          <Text style={styles.charCounter}>
            {state.comment.length}/{MAX_COMMENT_LENGTH}
          </Text>
        </View>

        <TouchableOpacity
          onPress={handleSubmit}
          style={[
            styles.button,
            state.loading && styles.buttonDisabled,
            { opacity: state.rating === 0 ? 0.6 : 1 }
          ]}
          disabled={state.loading || state.rating === 0}
        >
          {state.loading ? (
            <ActivityIndicator color={colors.white} />
          ) : (
            <Text style={styles.buttonText}>
              {state.evaluationId ? 'Atualizar Avaliação' : 'Enviar Avaliação'}
            </Text>
          )}
        </TouchableOpacity>

        {state.evaluationId && (
          <TouchableOpacity
            onPress={() => setShowDeleteConfirmation(true)}
            style={[styles.button, styles.deleteButton, state.loading && styles.buttonDisabled]}
            disabled={state.loading}
          >
            <Text style={styles.deleteButtonText}>Excluir Avaliação</Text>
          </TouchableOpacity>
        )}
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: colors.blue,
  },
  content: {
    padding: 20,
    flex: 1,
  },
  loadingContainer: {
    justifyContent: 'center',
    alignItems: 'center',
  },
  ratingContainer: {
    marginBottom: 24,
    alignItems: 'center',
  },
  ratingText: {
    marginTop: 8,
    fontSize: 16,
    color: colors.white,
    fontWeight: '500',
  },
  label: {
    fontSize: 16,
    color: colors.white,
    marginBottom: 8,
    fontWeight: '500',
  },
  inputContainer: {
    position: 'relative',
    marginBottom: 24,
  },
  input: {
    backgroundColor: colors.white,
    borderRadius: 12,
    padding: 16,
    fontSize: 16,
    textAlignVertical: 'top',
    color: colors.black,
    minHeight: 120,
    elevation: 2,
  },
  charCounter: {
    position: 'absolute',
    right: 8,
    bottom: 8,
    color: colors.grayHard,
    fontSize: 12,
  },
  button: {
    backgroundColor: colors.green,
    paddingVertical: 16,
    borderRadius: 12,
    alignItems: 'center',
    elevation: 3,
  },
  deleteButton: {
    backgroundColor: '#FF0000',
    marginTop: 12,
  },
  buttonDisabled: {
    opacity: 0.6,
  },
  buttonText: {
    fontWeight: 'bold',
    fontSize: 16,
    color: colors.white,
  },
  deleteButtonText: {
    fontWeight: 'bold',
    fontSize: 16,
    color: colors.white,
  },
});