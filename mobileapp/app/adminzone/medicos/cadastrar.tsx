import { useState, useEffect } from "react";
import {
  View,
  ScrollView,
  Text,
  KeyboardAvoidingView,
  Platform,
  TouchableOpacity,
  Alert,
  ActivityIndicator
} from "react-native";
import { useForm, Controller } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import Toast from "react-native-root-toast";
import Button from "@/components/ui/Button";
import TextInput from "@/components/ui/TextInput";
import { useRouter } from "expo-router";
import { HttpClient } from "@/services/restrict/HttpClient";
import { USER_ACCESS_TOKEN_NAME } from "@/contexts/AdminAuthenticationContext";
import { MedicService } from "@/services/restrict/MedicService";
import { AdminAppointmentsService } from "@/services/restrict/AdminAppointmentService";
import { z } from "zod";
import { Ionicons } from '@expo/vector-icons';

interface IMedic {
  name: string;
  cpf: string;
  email: string;
  crm: string;
  specializationId: number;
  units: any[];
}

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

const formatCPF = (cpf: string): string => {
  cpf = cpf.replace(/\D/g, ""); // Remove todos os caracteres não numéricos
  if (cpf.length > 11) cpf = cpf.slice(0, 11); // Limita o CPF a 11 dígitos
  return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4"); // Formata o CPF
};

const formatCPFForDatabase = (cpf: string): string => {
  return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4"); // Formata o CPF para o padrão 999.999.999-99
};

const removeCPFFormatting = (cpf: string): string => {
  return cpf.replace(/\D/g, '');
};

const formatCRM = (text: string): string => {
  // Remove todos os caracteres não alfanuméricos
  let cleaned = text.replace(/[^a-zA-Z0-9/]/g, "");

  // Converte para maiúsculas
  cleaned = cleaned.toUpperCase();

  // Extrai números e letras
  const numbers = cleaned.replace(/[^\d]/g, "");
  const letters = cleaned.replace(/[^A-Z]/g, "").slice(0, 2);

  // Permite 4 a 6 dígitos para o CRM
  const limitedNumbers = numbers.slice(0, 6);

  // Adiciona a barra automaticamente após os números
  if (limitedNumbers.length > 0 && letters.length > 0) {
    return `${limitedNumbers}/${letters}`;
  }

  // Se não há letras ainda, apenas retorna os números
  return limitedNumbers;
};

const processIds = (idsString: string): number[] => {
  return idsString.split(',')
    .map(id => id.trim())
    .filter(id => id)
    .map(id => parseInt(id))
    .filter(id => !isNaN(id));
};

export default function CadastrarMedico() {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [specializations, setSpecializations] = useState<Array<{ id: number; name: string }>>([]);
  const [showSpecializationDropdown, setShowSpecializationDropdown] = useState(false);

  const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
  const medicoService = new MedicService(client);
  const appointmentService = new AdminAppointmentsService(client);

  const {
    control,
    handleSubmit,
    setValue,
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

  useEffect(() => {
    const loadSpecializations = async () => {
      try {
        const response = await appointmentService.getAllSpecializationsAsync();
        if (response.data?.success) {
          const specs = response.data.data;
          // Ordenar as especializações alfabeticamente pelo nome
          const sortedSpecs = [...specs].sort((a, b) => a.name.localeCompare(b.name));
          setSpecializations(sortedSpecs);
          if (sortedSpecs.length > 0) {
            setValue("especialidade", sortedSpecs[0].id.toString());
          }
        }
      } catch (err) {
        console.error("Erro ao carregar especializações:", err);
        setError("Erro ao carregar especializações. Por favor, tente novamente.");
      }
    };
    loadSpecializations();
  }, []);

  const onSubmit = async (data: DoctorFormData) => {
    try {
      setLoading(true);
      setError("");      const payload: IMedic = {
        name: data.nome.trim(),
        cpf: formatCPFForDatabase(removeCPFFormatting(data.cpf.toString())),
        email: data.email.trim(),
        crm: data.crm, // Manter o formato com barra (ex: 1234/MG)
        specializationId: parseInt(data.especialidade),
        units: [],
      };

      console.log('Payload formatado para envio:', JSON.stringify(payload, null, 2));      // Make API request
      const result = await medicoService.createAsync(payload);

      console.log('Resultado do cadastro:', result);
      
      // Show success message with Toast
      Toast.show('Médico cadastrado com sucesso!', {
        duration: 3000, // Duração de 3 segundos
        position: Toast.positions.BOTTOM,
        shadow: true,
        animation: true,
        hideOnPress: true,
        delay: 0,
        backgroundColor: '#8FDB5F',
        textColor: '#113F72'
      });

      // Aguarda 3 segundos antes de redirecionar
      setTimeout(() => {
        router.push("/adminzone/medicos");
      }, 3000);

    } catch (error: any) {
      console.error('Erro detalhado ao cadastrar médico:', error);

      if (error.response) {
        console.error('Status:', error.response.status);
        console.error('Headers:', error.response.headers);
        console.error('Detalhes da resposta:', error.response.data);

        // Handle validation errors
        const validationErrors = error.response.data?.errors || error.response.data?.message;
        if (validationErrors) {
          const errorMsg = typeof validationErrors === 'string' 
            ? validationErrors 
            : Object.entries(validationErrors)
                .map(([field, msgs]) => `${field}: ${msgs}`)
                .join('\n');

          setError(errorMsg);          Toast.show(errorMsg, {
            duration: Toast.durations.LONG,
            position: Toast.positions.TOP,
            shadow: true,
            animation: true,
            hideOnPress: true,
            delay: 0,
            backgroundColor: '#FF4040',
            textColor: '#FFFFFF',
          });
          return;
        }
      }

      const errorMessage = error.message || 'Ocorreu um erro ao cadastrar o médico. Verifique os dados e tente novamente.';
      setError(errorMessage);      Toast.show(errorMessage, {
        duration: Toast.durations.LONG,
        position: Toast.positions.TOP,
        shadow: true,
        animation: true,
        hideOnPress: true,
        delay: 0,
        backgroundColor: '#FF4040',
        textColor: '#FFFFFF',
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
                          {specializations.find(s => s.id.toString() === value)?.name || "Selecione uma especialidade"}
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
                  render={({ field: { onChange, value } }) => (
                    <TextInput
                      placeholder="1234/UF ou 123456/UF"
                      value={value}
                      onChangeText={(text) => onChange(formatCRM(text))}
                      error={!!errors.crm}
                      errorMessage={errors.crm?.message}
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
                  {loading ? 'Cadastrando...' : 'Cadastrar Médico'}
                </Text>
              </TouchableOpacity>
            </View>
          </View>
        </ScrollView>
      </KeyboardAvoidingView>
    </View>
  );
}