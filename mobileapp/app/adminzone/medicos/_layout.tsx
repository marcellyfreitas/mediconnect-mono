import { Stack } from 'expo-router';
import { DrawerToggleButton } from '@react-navigation/drawer';
import { colors } from '@/utils/constants';

type RouteParams = {
  specializationName?: string;
};

// Layout para a tela de médicos
// Este layout define as telas que podem ser acessadas a partir da tela de médicos
export default function MedicosLayout() {
  return (
    <Stack initialRouteName="index">
      <Stack.Screen
        name="index"
        options={{
          headerShown: true,
          title: 'Médicos Credenciados',
          headerStyle: {
            backgroundColor: colors.green,
          },
          headerTintColor: colors.blue,
          headerLeft: ({ tintColor }) => (<DrawerToggleButton tintColor={tintColor} />),
        }}
      />
      <Stack.Screen
        name="especialidade"
        options={{
          headerShown: true,
          title: 'Médicos por Especialidade',
          headerStyle: {
            backgroundColor: colors.green,
          },
          headerTintColor: colors.blue,
        }}
      />
      <Stack.Screen
        name="detalhes"
        options={({ route }) => ({
          headerShown: true,
          title: (route.params as RouteParams)?.specializationName || 'Especialidade',
          headerStyle: {
            backgroundColor: colors.green,
          },
          headerTintColor: colors.blue,
        })}
      />
      <Stack.Screen
        name="editar"
        options={{
          headerShown: true,
          title: 'Editar Médico',
          headerStyle: {
            backgroundColor: colors.green,
          },
          headerTintColor: colors.blue,
        }}
      />
      <Stack.Screen
        name="cadastrar"
        options={{
          headerShown: true,
          title: 'Cadastrar Médico',
          headerStyle: {
            backgroundColor: colors.green,
          },
          headerTintColor: colors.blue,
        }}
      />
    </Stack>
  );
}
