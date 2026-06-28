import { useState, useEffect } from "react";
import {
  View,
  ScrollView,
  Text,
  KeyboardAvoidingView,
  Platform,
  TouchableOpacity,
  ActivityIndicator
} from "react-native";
import { useForm, Controller } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import Toast from "react-native-root-toast";
import Button from "@/components/ui/Button";
import TextInput from "@/components/ui/TextInput";
import { useLocalSearchParams, useRouter } from "expo-router";
import { HttpClient } from "@/services/restrict/HttpClient";
import { USER_ACCESS_TOKEN_NAME } from "@/contexts/AdminAuthenticationContext";
import { MedicService } from "@/services/restrict/MedicService";
import { AdminAppointmentsService } from "@/services/restrict/AdminAppointmentService";
import { z } from "zod";
import { Ionicons } from '@expo/vector-icons';

// Schema de validação do formulário
const doctorFormSchema = z.object({
  nome: z.string().min(1, "Nome é obrigatório"),
  email: z.string().email("E-mail inválido"),
  cpf: z
    .string()
    .min(14, "CPF deve estar no formato 999.999.999-99")
    .max(14, "CPF deve estar no formato 999.999.999-99")
    .regex(/^\d{3}\.\d{3}\.\d{3}-\d{2}$/, "CPF deve estar no formato 999.999.999-99"),
  especialidade: z.string().min(1, "Especialidade é obrigatória"),  crm: z
    .string()
    .min(6, "CRM deve estar no formato 1234/UF ou 123456/UF")
    .regex(/^\d{4,6}\/[A-Z]{2}$/, "CRM deve estar no formato 1234/UF ou 123456/UF"),
});

type DoctorFormData = z.infer<typeof doctorFormSchema>;

// Funções utilitárias para formatação
const formatCPF = (cpf: string): string => {
  cpf = cpf.replace(/\D/g, "");
  if (cpf.length > 11) cpf = cpf.slice(0, 11);
  return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
};

const formatCPFForDatabase = (cpf: string): string => {
  return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
};

const removeCPFFormatting = (cpf: string): string => {
  return cpf.replace(/\D/g, '');
};

const formatCRM = (crm: string) => {
  // Remove todos os caracteres não alfanuméricos
  let cleaned = crm.replace(/[\W_]/g, "");
  
  // Converte para maiúsculas
  cleaned = cleaned.toUpperCase();
  
  // Extrai números e letras
  const numbers = cleaned.replace(/[^\d]/g, "");
  const letters = cleaned.replace(/[^A-Z]/g, "").slice(0, 2);
  
  // Permite 4 a 6 dígitos para o CRM
  const limitedNumbers = numbers.slice(0, 6);
  
  // Se não houver letras, assume MG como padrão
  const state = letters || 'MG';
  
  // Retorna no formato adequado (1234/MG ou 123456/MG)
  return limitedNumbers ? `${limitedNumbers}/${state}` : '';
};

export default function MedicoEditScreen() {
  const params = useLocalSearchParams();
  const { ...routeParams } = params;
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [specializations, setSpecializations] = useState<Array<{ id: number; name: string }>>([]);
  const [showSpecializationDropdown, setShowSpecializationDropdown] = useState(false);
  const [currentSpecialization, setCurrentSpecialization] = useState<{ id: number; name: string } | null>(null);

  const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
  const medicoService = new MedicService(client);
  const appointmentService = new AdminAppointmentsService(client);

  const {
    control,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<DoctorFormData>({
    resolver: zodResolver(doctorFormSchema),
    defaultValues: {
      nome: "",
      email: "",
      cpf: "",
      especialidade: "",
      crm: "",
    }
  });

  // Carregar especializações
  useEffect(() => {
    const loadSpecializations = async () => {
      try {
        const response = await appointmentService.getAllSpecializationsAsync();
        if (response.data?.success) {
          const specs = response.data.data;
          // Ordenar as especializações alfabeticamente pelo nome
          const sortedSpecs = [...specs].sort((a, b) => a.name.localeCompare(b.name));
          setSpecializations(sortedSpecs);
        }
      } catch (err) {
        console.error("Erro ao carregar especializações:", err);
        setError("Erro ao carregar especializações. Por favor, tente novamente.");
      }
    };
    loadSpecializations();
  }, []);

  // Buscar dados do médico ao montar a tela
  useEffect(() => {
    const fetchMedico = async () => {
      if (!routeParams.id) return;
      setLoading(true);
      setError("");
      try {
        const medico = await medicoService.getByIdAsync(Number(routeParams.id));
        
        if (medico.specializationId) {
          const specs = await appointmentService.getAllSpecializationsAsync();
          if (specs.data?.success) {
            const allSpecs = specs.data.data.sort((a: any, b: any) => 
              a.name.localeCompare(b.name)
            );
            
            const currentSpec = allSpecs.find((s: any) => s.id === medico.specializationId);
            if (currentSpec) {
              setCurrentSpecialization(currentSpec);
            }
            
            setSpecializations(allSpecs);
          }
        }

        reset({
          nome: medico.name || "",
          cpf: formatCPF(medico.cpf || ""),
          email: medico.email || "",
          especialidade: medico.specializationId?.toString() || "",
          crm: formatCRM(medico.crm || ""),
        });
      } catch (err: any) {
        console.error("Erro ao buscar dados do médico:", err);
        setError("Erro ao buscar dados do médico.");
      } finally {
        setLoading(false);
      }
    };
    fetchMedico();
  }, [routeParams.id]);

  const onSubmit = async (data: DoctorFormData) => {
    try {
      setLoading(true);
      setError("");

      if (!data.especialidade) {
        throw new Error("Especialização é obrigatória");
      }      const payload = {
        name: data.nome.trim(),
        cpf: formatCPFForDatabase(removeCPFFormatting(data.cpf.toString())),
        email: data.email.trim(),
        crm: data.crm, // Manter o formato com barra (ex: 1234/MG)
        specializationId: Number(data.especialidade),
        units: [],
        id: Number(routeParams.id),
      };

      console.log("Payload enviado para o backend:", payload);

      const response = await medicoService.updateAsync(Number(routeParams.id), payload);

      if (response) {
        Toast.show('Médico atualizado com sucesso!', {
          duration: Toast.durations.SHORT,
          position: Toast.positions.BOTTOM,
          backgroundColor: '#8FDB5F',
          textColor: '#113F72'
        });
        router.replace({ pathname: '/adminzone/medicos', params: { refresh: Date.now() } });
      } else {
        throw new Error('Erro: Resposta do backend está vazia ou inválida.');
      }
    } catch (error: any) {
      console.error("Erro ao atualizar médico:", error);
      const errorMessage = error.message || 'Erro ao atualizar médico';
      setError(errorMessage);
      Toast.show(errorMessage, {
        duration: Toast.durations.LONG,
        position: Toast.positions.BOTTOM,
        backgroundColor: '#ff0000',
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <View className="flex-1 bg-[#113F72]">
      <KeyboardAvoidingView
        behavior={Platform.OS === "ios" ? "padding" : "height"}
        style={{ flex: 1 }}
      >
        <ScrollView className="flex-1">
          <View className="p-4">
            {!!error && <Text className="text-red-500 mb-4 p-3 bg-red-100 rounded">{error}</Text>}

            {loading ? (
              <View className="items-center justify-center p-4">
                <ActivityIndicator size="large" color="#113F72" />
                <Text className="mt-4 text-blue-900">Carregando dados...</Text>
              </View>
            ) : (
              <View className="space-y-4">
                <View>
                  <Text className="text-white font-bold mb-1">Nome</Text>
                  <Controller
                    control={control}
                    name="nome"
                    render={({ field: { onChange, value } }) => (
                      <TextInput
                        placeholder="Nome do médico"
                        value={value}
                        onChangeText={onChange}
                        error={!!errors.nome}
                        errorMessage={errors.nome?.message}
                      />
                    )}
                  />
                </View>

                <View>
                  <Text className="text-white font-bold mb-1">CPF</Text>
                  <Controller
                    control={control}
                    name="cpf"
                    render={({ field: { onChange, value } }) => (
                      <TextInput
                        placeholder="000.000.000-00"
                        value={value}
                        onChangeText={(text) => onChange(formatCPF(text))}
                        error={!!errors.cpf}
                        errorMessage={errors.cpf?.message}
                        keyboardType="numeric"
                        maxLength={14}
                      />
                    )}
                  />
                </View>

                <View>
                  <Text className="text-white font-bold mb-1">E-mail</Text>
                  <Controller
                    control={control}
                    name="email"
                    render={({ field: { onChange, value } }) => (
                      <TextInput
                        placeholder="email@exemplo.com"
                        value={value}
                        onChangeText={onChange}
                        error={!!errors.email}
                        errorMessage={errors.email?.message}
                        keyboardType="email-address"
                        autoCapitalize="none"
                      />
                    )}
                  />
                </View>

                <View>
                  <Text className="text-white font-bold mb-1">Especialidade</Text>
                  <Controller
                    control={control}
                    name="especialidade"
                    render={({ field: { value, onChange } }) => (
                      <>
                        <TouchableOpacity
                          onPress={() => setShowSpecializationDropdown(!showSpecializationDropdown)}
                          className="bg-white border border-gray-300 rounded-lg p-3 flex-row items-center justify-between"
                        >
                          <Text className="text-gray-700">
                            {currentSpecialization?.name || specializations.find(s => s.id.toString() === value)?.name || "Selecione uma especialidade"}
                          </Text>
                          <Ionicons name={showSpecializationDropdown ? "chevron-up" : "chevron-down"} size={20} color="#666" />
                        </TouchableOpacity>
                        {showSpecializationDropdown && (
                          <View className="bg-white border border-gray-300 rounded-lg mt-1 max-h-40">
                            <ScrollView>
                              {specializations.map((spec) => (
                                <TouchableOpacity
                                  key={spec.id}
                                  onPress={() => {
                                    onChange(spec.id.toString());
                                    setCurrentSpecialization(spec);
                                    setShowSpecializationDropdown(false);
                                  }}
                                  className="p-3 border-b border-gray-200"
                                >
                                  <Text className="text-gray-700">{spec.name}</Text>
                                </TouchableOpacity>
                              ))}
                            </ScrollView>
                          </View>
                        )}
                      </>
                    )}
                  />
                  {errors.especialidade && (
                    <Text className="text-red-500 text-sm mt-1">{errors.especialidade.message}</Text>
                  )}
                </View>

                <View>
                  <Text className="text-white font-bold mb-1">CRM</Text>
                  <Controller
                    control={control}
                    name="crm"
                    render={({ field: { onChange, value } }) => (                      <TextInput
                        placeholder="1234/UF ou 123456/UF"
                        value={value}
                        onChangeText={(text) => onChange(formatCRM(text))}
                        error={!!errors.crm}
                        errorMessage={errors.crm?.message}
                        maxLength={9}
                      />
                    )}
                  />
                </View>

                <TouchableOpacity
                  onPress={handleSubmit(onSubmit)}
                  disabled={loading}
                  className={`mt-6 rounded-lg p-4 flex-row items-center justify-center ${loading ? 'bg-gray-500' : 'bg-[#8FDB5F]'}`}
                >
                  <Ionicons name="save-outline" size={20} color="#113F72" />
                  <Text className="font-bold ml-2 text-[#113F72]">
                  {loading ? 'Atualizando...' : 'Atualizar Dados'}
                  </Text>
                </TouchableOpacity>
              </View>
            )}
          </View>
        </ScrollView>
      </KeyboardAvoidingView>
    </View>
  );
}