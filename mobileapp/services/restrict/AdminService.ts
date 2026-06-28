import { HttpClient } from '@/@types/IHttpClient';

export class AdminService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async addFromAdminAsync(data: any): Promise<any> {
		return this.client.post('/administradores', data);
	}

	async getAllFromAdminAsync(): Promise<any> {
		return this.client.get('/administradores');
	}

	async getByIdFromAdminAsync(id: number): Promise<any> {
		return this.client.get(`/administradores/${id}`);
	}

	async updateFromAdminAsync(id: number, data: any) {
		return this.client.put(`/administradores/${id}`, data);
	}

	async deleteFromAdminAsync(id: number) {
		return this.client.delete(`/administradores/${id}`);
	}
};