import { ThemedText } from "@/components/ui/ThemedText";
import { ThemedView } from "@/components/ui/ThemedView";
import Button from "@/components/ui/Button";
import { useCallback, useEffect, useState } from "react";
import { HttpClient } from "@/services/restrict/HttpClient";
import { USER_ACCESS_TOKEN_NAME } from "@/contexts/AdminAuthenticationContext";
import {
  ActivityIndicator,
  FlatList,
  RefreshControl,
  Text,
  TextInput,
} from "react-native";
import Toast from "react-native-root-toast";
import { useActionSheet } from "@expo/react-native-action-sheet";
import CardCrud from "@/components/ui/CardCrud";
import { MedicalAgreementService } from "@/services/restrict/MedicalAgreementService";

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new MedicalAgreementService(client);

const ConveniosScreen = () => {
  const [list, setList] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [novoConvenio, setNovoConvenio] = useState("");
  const [editItem, setEditItem] = useState(null);
  const { showActionSheetWithOptions } = useActionSheet();

  const fetchConvenios = useCallback(async () => {
    try {
      setLoading(true);
      const { data: response } = await service.getAllFromAdminAsync();
      setList(response.data);
      setError(null);
    } catch (err) {
      console.error("Erro ao buscar convênios:", err);
      setList([]);
    } finally {
      setLoading(false);
    }
  }, []);

  const onRefresh = useCallback(() => {
    fetchConvenios();
  }, [fetchConvenios]);

  useEffect(() => {
    fetchConvenios();
  }, [fetchConvenios]);

  async function save() {
    if (editItem) {
      const { id } = editItem;
      await service.updateFromAdminAsync(id, {
        name: novoConvenio,
      });
    } else {
      await service.addFromAdminAsync({
        name: novoConvenio,
      });
    }

    Toast.show("Operação realizada com sucesso!", {
      duration: Toast.durations.SHORT,
      position: Toast.positions.BOTTOM,
      animation: true,
    });

    setNovoConvenio("");
    setEditItem(null);
    fetchConvenios();
  }

  const deleteItem = useCallback(
    async (item: any) => {
      try {
        await service.deleteFromAdminAsync(item.id);
        Toast.show("Operação realizada com sucesso!", {
          duration: Toast.durations.SHORT,
          position: Toast.positions.BOTTOM,
          animation: true,
        });
        fetchConvenios();
      } catch (err) {
        console.error("Erro ao deletar item:", err);
      }
    },
    [fetchConvenios]
  );

  const onPressDelete = useCallback(
    (item: any) => {
      const options = ["Deletar", "Cancelar"];
      const destructiveButtonIndex = 0;
      const cancelButtonIndex = 1;

      showActionSheetWithOptions(
        {
          options,
          cancelButtonIndex,
          destructiveButtonIndex,
        },
        (selected) => {
          if (selected === destructiveButtonIndex) {
            deleteItem(item);
          }
        }
      );
    },
    [deleteItem, showActionSheetWithOptions]
  );

  function handleEdit(item: any) {
    setEditItem(item);
    setNovoConvenio(item.name);
  }

  function cancelEdit() {
    setEditItem(null);
    setNovoConvenio("");
  }

  return (
    <ThemedView className="relative flex-1 w-full p-5">
      <ThemedView className="flex flex-row mb-4">
        <TextInput
          className="flex-1 px-4 py-2 mr-2 text-black bg-white border border-gray-300 rounded-lg"
          placeholder="Nome do convênio"
          placeholderTextColor="#CCCCCC"
          value={novoConvenio}
          onChangeText={setNovoConvenio}
          onSubmitEditing={save}
          returnKeyType="done"
        />
        <Button onPress={save} color="primary" className="px-4 py-2">
          <ThemedText>{editItem ? "Salvar" : "Adicionar"}</ThemedText>
        </Button>
        {editItem && (
          <Button
            onPress={cancelEdit}
            color="danger"
            className="px-4 py-2 ml-2"
          >
            <ThemedText>Cancelar</ThemedText>
          </Button>
        )}
      </ThemedView>

      <ThemedView className="flex flex-col flex-1 w-full gap-4">
        {loading && <ActivityIndicator size="large" color="#007BFF" />}

        {error && <ThemedText className="text-center">{error}</ThemedText>}

        <FlatList
          data={list}
          keyExtractor={(item: any) => item.id.toString()}
          renderItem={({ item }: any) => (
            <CardCrud item={item} onDelete={onPressDelete} onEdit={handleEdit}>
              <Text className="text-lg font-semibold text-slate-600">
                {item.name}
              </Text>
            </CardCrud>
          )}
          contentContainerStyle={{ paddingBottom: 50, width: "100%", gap: 10 }}
          refreshControl={
            <RefreshControl
              refreshing={loading}
              onRefresh={onRefresh}
              colors={["#007BFF"]}
              progressBackgroundColor="#FFFFFF"
            />
          }
        />
      </ThemedView>
    </ThemedView>
  );
};

export default ConveniosScreen;