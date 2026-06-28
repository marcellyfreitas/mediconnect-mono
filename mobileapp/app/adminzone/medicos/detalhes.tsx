import {
  View,
  Text,
  ScrollView,
  TouchableOpacity,
  ActivityIndicator,
  Alert,
  Platform,
} from "react-native";
import { useLocalSearchParams, useRouter } from "expo-router";
import { useState, useEffect } from "react";
import { Ionicons } from "@expo/vector-icons";
import { HttpClient } from "@/services/restrict/HttpClient";
import { USER_ACCESS_TOKEN_NAME } from "@/contexts/AdminAuthenticationContext";
import { MedicService } from "@/services/restrict/MedicService";
import { AdminAppointmentsService } from "@/services/restrict/AdminAppointmentService";
import Toast from "react-native-root-toast";

interface Doctor {
  id: number;
  name: string;
  cpf: string;
  crm: string;
  createdAt: string;
  updatedAt: string;
  email: string;
  phone: string;
  specialization: {
    id: number;
    name: string;
  };
}

const formatDateTime = (dateString: string) => {
  const date = new Date(dateString);
  const day = date.getDate().toString().padStart(2, "0");
  const month = (date.getMonth() + 1).toString().padStart(2, "0");
  const year = date.getFullYear();
  const hours = date.getHours().toString().padStart(2, "0");
  const minutes = date.getMinutes().toString().padStart(2, "0");

  return `${day}/${month}/${year} às ${hours}:${minutes}.`;
};

const formatCRM = (crm: string) => {
  // Assuming CRM is in format "1234MG" or just numbers, convert to "1234/MG"
  const numbers = crm.replace(/[^\d]/g, "");
  const state = crm.replace(/[\d]/g, "").toUpperCase();
  return `${numbers}${state || "MG"}`;
};

const showConfirmDialog = (
  title: string,
  message: string,
  onConfirm: () => void,
  onCancel?: () => void
) => {
  if (Platform.OS === "web") {
    // Para web, usa confirm do navegador
    const confirmed = window.confirm(`${title}\n\n${message}`);
    if (confirmed) {
      onConfirm();
    } else if (onCancel) {
      onCancel();
    }
  } else {
    // Para mobile, usa Alert do React Native
    Alert.alert(
      title,
      message,
      [
        {
          text: "Cancelar",
          style: "cancel",
          onPress: onCancel,
        },
        {
          text: "Confirmar",
          style: "destructive",
          onPress: onConfirm,
        },
      ],
      { cancelable: false }
    );
  }
};

const showErrorDialog = (title: string, message: string) => {
  if (Platform.OS === "web") {
    // Para web, usa alert do navegador
    window.alert(`${title}\n\n${message}`);
  } else {
    // Para mobile, usa Alert do React Native
    Alert.alert(title, message, [{ text: "OK" }]);
  }
};

export default function DetalhesMedicos() {
  const params = useLocalSearchParams();
  const router = useRouter();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [doctors, setDoctors] = useState<Doctor[]>([]);
  const [specialization, setSpecialization] = useState<{
    id: number;
    name: string;
  } | null>(null);

  const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
  const medicService = new MedicService(client);
  const appointmentService = new AdminAppointmentsService(client);

  const loadData = async () => {
    try {
      setLoading(true);
      const [specsResponse, docsResponse] = await Promise.all([
        appointmentService.getAllSpecializationsAsync(),
        medicService.getAllAsync(),
      ]);

      if (specsResponse.data?.success) {
        const spec = specsResponse.data.data.find(
          (s: any) => s.id === Number(params.specializationId)
        );
        setSpecialization(spec);
      }

      if (docsResponse) {
        const filteredDocs = docsResponse.filter(
          (doc: Doctor) =>
            doc.specialization.id === Number(params.specializationId)
        );
        setDoctors(filteredDocs);
      }
    } catch (err) {
      setError("Erro ao carregar dados");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, [params.specializationId]);

  if (loading) {
    return (
      <View className="flex-1 justify-center items-center bg-[#113F72]">
        <ActivityIndicator size="large" color="#fff" />
      </View>
    );
  }

  return (
    <View className="flex-1 bg-[#113F72]">
      <ScrollView className="flex-1 p-4">
        {error ? (
          <Text className="text-red-500 text-center p-4 bg-white rounded-lg">
            {error}
          </Text>
        ) : doctors.length === 0 ? (
          <Text className="text-white text-center p-4">
            Nenhum médico encontrado para esta especialidade
          </Text>
        ) : (
          <View>
            {doctors.map((doctor, index) => (
              <TouchableOpacity
                key={doctor.id}
                className="p-4 bg-white rounded-lg mb-4 shadow-sm"
                onPress={() =>
                  router.push({
                    pathname: "/adminzone/medicos/editar",
                    params: { id: doctor.id },
                  })
                }
              >
                <Text className="text-[#113F72] font-semibold text-lg">
                  {doctor.name}
                </Text>
                <View className="mt-2">
                  <Text className="text-[#113F72]">CPF: {doctor.cpf}</Text>
                  <Text className="text-[#113F72]">
                    CRM: {formatCRM(doctor.crm)}
                  </Text>
                  <Text className="text-[#113F72]">E-mail: {doctor.email}</Text>
                  <Text className="text-[#113F72]">
                    Criado em: {formatDateTime(doctor.createdAt)}
                  </Text>
                  <Text className="text-[#113F72]">
                    Atualizado em: {formatDateTime(doctor.updatedAt)}
                  </Text>
                </View>
                <View className="flex-row justify-center mt-4 space-x-4">
                  <TouchableOpacity
                    className="bg-[#113F72] px-8 py-3 rounded-md flex-row items-center justify-center flex-1 max-w-[150]"
                    onPress={() =>
                      router.push({
                        pathname: "/adminzone/medicos/editar",
                        params: { id: doctor.id },
                      })
                    }
                  >
                    <Ionicons name="create-outline" size={20} color="white" />
                    <Text className="text-white ml-2 font-semibold">
                      Editar
                    </Text>
                  </TouchableOpacity>{" "}
                  <TouchableOpacity
                    className="bg-[#B22222] px-8 py-3 rounded-md flex-row items-center justify-center flex-1 max-w-[150]"
                    onPress={() => {
                      showConfirmDialog(
                        "Confirmar exclusão",
                        `Tem certeza que deseja excluir o médico(a) ${doctor.name}?`,
                        async () => {
                          try {
                            await medicService.deleteAsync(doctor.id);
                            Toast.show("Médico(a) excluído com sucesso!", {
                              duration: Toast.durations.SHORT,
                              position: Toast.positions.BOTTOM,
                              animation: true,
                              backgroundColor: "#8FDB5F",
                              textColor: "#113F72",
                            });
                            loadData();
                          } catch (err) {
                            showErrorDialog(
                              "Erro",
                              "Não foi possível excluir o médico(a). Tente novamente."
                            );
                            Toast.show("Erro ao excluir médico(a)!", {
                              duration: Toast.durations.LONG,
                              position: Toast.positions.BOTTOM,
                              animation: true,
                              backgroundColor: "#B22222",
                              textColor: "#FFFFFF",
                            });
                          }
                        }
                      );
                    }}
                  >
                    <Ionicons name="trash-outline" size={20} color="white" />
                    <Text className="text-white ml-2 font-semibold">
                      Excluir
                    </Text>
                  </TouchableOpacity>
                </View>
              </TouchableOpacity>
            ))}
          </View>
        )}
      </ScrollView>
    </View>
  );
}