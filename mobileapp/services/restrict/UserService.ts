import { HttpClient } from '@/@types/IHttpClient';

export class UserService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async addFromAdminAsync(data: any): Promise<any> {
		return this.client.post('/usuarios', data);
	}

	async getAllFromAdminAsync(): Promise<any> {
		return this.client.get('/usuarios');
	}

	async getByIdFromAdminAsync(id: number): Promise<any> {
		return this.client.get(`/usuarios/${id}`);
	}

	async updateFromAdminAsync(id: number, data: any) {
		return this.client.put(`/usuarios/${id}`, data);
	}

	async deleteFromAdminAsync(id: number) {
		return this.client.delete(`/usuarios/${id}`);
	}
};