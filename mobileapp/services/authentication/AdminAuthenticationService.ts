import type { HttpClient, IAdmin, IAuthenticationService } from '@/@types';


export class AdminAuthenticationService implements IAuthenticationService<IAdmin> {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async login(payload: any): Promise<{ accessToken: string, refreshToken: string }> {
		try {
			const response = await this.client.post('/auth/admin/login', {
				email: payload.username,
				password: payload.password,
			});

			const tokens = {
				accessToken: (response.data?.data?.accessToken ?? '') as string,
				refreshToken: (response.data?.data?.refreshToken ?? '') as string,
			};

			if (!tokens.accessToken || !tokens.refreshToken) {
				throw 'Não autorizado!';
			}

			return tokens;
		} catch (error) {
			console.error(error);
			throw error;
		}
	}

	async userData() {
		try {
			const { data } = await this.client.get('/auth/admin/usuario');

			if (!data?.data?.email) {
				throw 'Erro ao capturar dados de usuário';
			}

			const user: IAdmin = {
				id: data?.data?.id,
				email: data?.data?.email,
				name: data?.data?.name,
			};

			return user;
		} catch (error) {
			throw error;
		}

	}

	async register(data: any) {
		try {
			return this.client.post('/auth/admin/register', data);
		} catch (error) {
			console.error(error);
			throw '[Service]: Erro ao cadastrar usuário';
		}
	}

	async logout() {
		try {
			return this.client.post('/auth/admin/logout');
		} catch (error) {
			console.error(error);
			throw '[Service]: Erro ao deslogar usuário';
		}
	}
};