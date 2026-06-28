import { View, Text, TouchableOpacity, ActivityIndicator } from "react-native";
import { useLocalSearchParams, useRouter } from "expo-router";
import { useState, useEffect } from "react";
import { MedicService } from "@/services/restrict/MedicService";
import { HttpClient } from "@/services/restrict/HttpClient";
import { USER_ACCESS_TOKEN_NAME } from "@/contexts/AdminAuthenticationContext";
import { Ionicons } from '@expo/vector-icons';
import Toast from "react-native-root-toast";

export default function RemoverMedicoScreen() {
  const params = useLocalSearchParams();
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [medico, setMedico] = useState<any>(null);

  const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
  const service = new MedicService(client);

  useEffect(() => {
    const fetchMedico = async () => {
      if (!params.id) return;
      setLoading(true);
      try {
        const data = await service.getByIdAsync(Number(params.id));
        setMedico(data);
      } catch (err) {
        console.error('Error fetching doctor:', err);
        setError('Erro ao carregar dados do médico');
      } finally {
        setLoading(false);
      }
    };
    fetchMedico();
  }, [params.id]);

  const handleRemover = async () => {
    if (!params.id) return;
    setLoading(true);
    try {
      await service.deleteAsync(Number(params.id));
      Toast.show('Médico removido com sucesso!', {
        duration: Toast.durations.SHORT,
        position: Toast.positions.BOTTOM,
      });
      router.replace({ pathname: '/adminzone/medicos', params: { refresh: Date.now() } });
    } catch (err) {
      console.error('Error removing doctor:', err);
      setError('Erro ao remover médico');
      Toast.show('Erro ao remover médico', {
        duration: Toast.durations.LONG,
        position: Toast.positions.BOTTOM,
        backgroundColor: '#ff0000',
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <View className="flex-1 bg-white">
      {/* Cabeçalho */}
      <View className="bg-blue-900 p-4">
        <TouchableOpacity 
          onPress={() => router.back()}
          className="flex-row items-center mb-4"
        >
          <Ionicons name="arrow-back" size={24} color="#fff" />
          <Text className="text-white text-lg ml-2">Voltar</Text>
        </TouchableOpacity>
        <Text className="text-white text-2xl font-bold">
          Remover Médico
        </Text>
      </View>

      <View className="flex-1 p-4">
        {loading ? (
          <View className="flex-1 justify-center items-center">
            <ActivityIndicator size="large" color="#113F72" />
            <Text className="mt-4 text-blue-900">Carregando...</Text>
          </View>
        ) : error ? (
          <View className="bg-red-100 p-4 rounded">
            <Text className="text-red-600">{error}</Text>
          </View>
        ) : medico ? (
          <View className="flex-1">
            <View className="bg-red-50 p-6 rounded-lg mb-6">
              <Ionicons name="warning" size={48} color="#DC2626" className="mb-4" />
              <Text className="text-lg text-red-600 font-bold mb-2">
                Confirmação de Exclusão
              </Text>
              <Text className="text-gray-700 mb-4">
                Você está prestes a remover permanentemente o médico:
              </Text>
              <Text className="text-xl font-bold text-gray-900 mb-1">
                {medico.name}
              </Text>
              <Text className="text-gray-600">
                CRM: {medico.crm}
              </Text>
              <Text className="text-gray-600">
                Especialidade: {medico.specialization?.name}
              </Text>
            </View>

            <Text className="text-red-600 font-bold mb-6">
              Esta ação não pode ser desfeita. Todos os dados relacionados a este médico serão excluídos permanentemente.
            </Text>

            <TouchableOpacity
              onPress={handleRemover}
              disabled={loading}
              className={`bg-red-600 rounded-lg p-4 flex-row items-center justify-center mb-4 ${loading ? 'opacity-50' : ''}`}
            >
              <Ionicons name="trash-outline" size={20} color="#fff" />
              <Text className="text-white font-bold ml-2">
                {loading ? 'Removendo...' : 'Confirmar Remoção'}
              </Text>
            </TouchableOpacity>

            <TouchableOpacity
              onPress={() => router.back()}
              className="bg-gray-200 rounded-lg p-4 flex-row items-center justify-center"
            >
              <Ionicons name="close-outline" size={20} color="#374151" />
              <Text className="text-gray-700 font-bold ml-2">Cancelar</Text>
            </TouchableOpacity>
          </View>
        ) : (
          <Text className="text-center text-gray-600">
            Médico não encontrado
          </Text>
        )}
      </View>
    </View>
  );
}