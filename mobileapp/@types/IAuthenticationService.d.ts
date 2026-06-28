export interface IAuthenticationService<T> {
	login: (data: any) => Promise<{ accessToken: string, refreshToken: string }>,
	userData: () => Promise<T>,
	register: (data: { name: string, cpf: string, email: string, password: string }) => Promise<any>,
	logout: () => Promise<any>,
}