import { View, Text, ScrollView, TouchableOpacity } from 'react-native';
import { useLocalSearchParams, useRouter } from 'expo-router';
import { useState, useEffect } from 'react';
import { MedicService, IMedic } from '@/services/restrict/MedicService';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { Ionicons } from '@expo/vector-icons';

interface Doctor extends IMedic {
  id?: number;
  createdAt?: string;
  updatedAt?: string;
}

export default function EspecialidadeScreen() {
  const { especialidade } = useLocalSearchParams();
  const router = useRouter();
  const [medicos, setMedicos] = useState<Doctor[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
  const service = new MedicService(client);

  useEffect(() => {
    carregarMedicos();
  }, []);

  const carregarMedicos = async () => {
    try {
      setLoading(true);
      const response = await service.getAllAsync();
      if (Array.isArray(response)) {
        // Filtra médicos pela especialidade usando o specializationId
        const data = response.filter((medico: Doctor) => 
          medico.specializationId === Number(especialidade)
        );
        setMedicos(data);
        setError('');
      } else {
        setMedicos([]);
        setError('Não foi possível carregar a lista de médicos.');
      }
    } catch (err) {
      console.error('Erro ao carregar médicos:', err);
      setError('Erro ao carregar médicos. Por favor, tente novamente.');
      setMedicos([]);
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
          {especialidade}
        </Text>
      </View>

      {/* Lista de médicos */}
      <ScrollView className="flex-1 p-4">
        {loading ? (
          <Text className="text-center">Carregando...</Text>
        ) : error ? (
          <View className="bg-red-100 p-4 rounded">
            <Text className="text-red-600">{error}</Text>
            <TouchableOpacity onPress={carregarMedicos} className="mt-2">
              <Text className="text-blue-600">Tentar novamente</Text>
            </TouchableOpacity>
          </View>
        ) : medicos.length === 0 ? (
          <Text className="text-center text-gray-600">
            Nenhum médico encontrado para esta especialidade.
          </Text>
        ) : (
          <View>
            {medicos.map((medico) => (
              <TouchableOpacity 
                key={medico.id}
                onPress={() => router.push({
                  pathname: '/adminzone/medicos/detalhes',
                  params: { id: medico.id }
                })}
                className="bg-gray-50 rounded-lg p-4 mb-4 shadow border border-gray-200"
              >
                <Text className="text-lg font-semibold text-blue-900 mb-1">
                  {medico.name}
                </Text>
                <Text className="text-gray-600 mb-1">
                  CRM: {medico.crm}
                </Text>
                <View className="flex-row justify-end space-x-2 mt-2">
                  <TouchableOpacity
                    onPress={() => router.push({
                      pathname: '/adminzone/medicos/detalhes',
                      params: { id: medico.id }
                    })}
                    className="flex-row items-center bg-blue-900 rounded px-3 py-1"
                  >
                    <Text className="text-white">Detalhes</Text>
                  </TouchableOpacity>
                </View>
              </TouchableOpacity>
            ))}
          </View>
        )}
      </ScrollView>

      {/* Botão flutuante para adicionar */}
      <TouchableOpacity
        onPress={() => router.push('/adminzone/medicos/cadastrar')}
        className="absolute bottom-6 right-6 bg-lime-400 w-14 h-14 rounded-full items-center justify-center shadow-lg"
      >
        <Ionicons name="add" size={32} color="#001F54" />
      </TouchableOpacity>
    </View>
  );
}