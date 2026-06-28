import { View, Text, TextInput, ScrollView, TouchableOpacity } from 'react-native';
import { useRouter, useLocalSearchParams } from 'expo-router';
import { Ionicons } from '@expo/vector-icons';
import { useState, useEffect } from 'react';
import { MedicService, IMedic } from '@/services/restrict/MedicService';
import { HttpClient } from '@/services/restrict/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { normalizeText } from '@/utils/doctors';
import { AdminAppointmentsService } from '@/services/restrict/AdminAppointmentService';
import { colors } from '@/utils/constants';

interface Doctor extends IMedic {
  id?: number;
  createdAt?: string;
  updatedAt?: string;
  specialization?: {
    name: string;
  };
}

interface Specialization {
  id: number;
  name: string;
}

export default function MedicosIndex() {
  const params = useLocalSearchParams();
  const router = useRouter();
  const [busca, setBusca] = useState('');
  const [medicos, setMedicos] = useState<Doctor[]>([]);
  const [specializations, setSpecializations] = useState<Specialization[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
  const service = new MedicService(client);
  const appointmentService = new AdminAppointmentsService(client);

  const carregarEspecializacoes = async () => {
    try {
      setLoading(true);
      const response = await appointmentService.getAllSpecializationsAsync();
      if (response.data?.success) {
        const specs = response.data.data;
        // Sort specializations alphabetically
        setSpecializations(specs.sort((a: Specialization, b: Specialization) => 
          a.name.localeCompare(b.name)
        ));
      }
    } catch (err) {
      console.error('Erro ao carregar especializações:', err);
      setError('Erro ao carregar especializações');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    carregarEspecializacoes();
  }, [params.refresh]);

  const especializacoesFiltradas = specializations.filter((spec) =>
    normalizeText(spec.name).includes(normalizeText(busca))
  );

  return (
    <View className="flex-1" style={{ backgroundColor: colors.blue }}>
      {/* Cabeçalho com título e barra de pesquisa */}
      <View className="p-4 shadow-md" style={{ backgroundColor: colors.blue }}>
        <View className="flex-row items-center bg-white rounded-md p-2">
          <Ionicons name="search" size={20} color={colors.blue} />
          <TextInput
            className="flex-1 ml-2"
            placeholder="Buscar especialidade..."
            placeholderTextColor={colors.blue}
            value={busca}
            onChangeText={setBusca}
          />
        </View>
      </View>

      {/* Lista de especialidades e médicos */}
      <ScrollView className="flex-1" style={{ backgroundColor: colors.blue }}>
        {loading ? (
          <Text className="text-center p-4 text-white">Carregando...</Text>
        ) : error ? (
          <Text className="text-center text-red-500 p-4">{error}</Text>
        ) : (
          <View className="p-4">
            {especializacoesFiltradas.length === 0 ? (
              <Text className="text-center p-4 text-white">Nenhuma especialidade encontrada</Text>
            ) : (
              especializacoesFiltradas.map((spec) => (
                <TouchableOpacity 
                  key={spec.id}
                  className="p-4 bg-white rounded-lg mb-4 shadow-sm"
                  onPress={() => router.push({
                    pathname: '/adminzone/medicos/detalhes',
                    params: { 
                      specializationId: spec.id,
                      specializationName: spec.name 
                    }
                  })}
                >
                    <Text style={{ color: colors.blue }} className="font-bold">{spec.name}</Text>
                    <View className="flex-row items-center mt-1">
                    <Ionicons name="medkit" size={16} color={colors.blue} />
                    <Text style={{ color: colors.blue }} className="ml-2 font-semibold">Ver médicos disponíveis</Text>
                    </View>
                </TouchableOpacity>
              ))
            )}
          </View>
        )}
      </ScrollView>

      {/* Botão flutuante para adicionar */}
      <TouchableOpacity
        onPress={() => router.push('/adminzone/medicos/cadastrar')}
        className="absolute bottom-6 right-6 w-14 h-14 rounded-full items-center justify-center shadow-lg"
        style={{ backgroundColor: colors.green }}
      >
        <Ionicons name="add" size={32} color={colors.blue} />
      </TouchableOpacity>
    </View>
  );
}