// filepath: client/src/services/authService.js
import api from "../utils/api.js";

export const authService = {
	async googleAuth(idToken) {
		try {
			const response = await api.post("/auth/google", { idToken });
			return response.data;
		} catch (error) {
			throw new Error(error.response?.data?.message || "Authentication failed");
		}
	},

	logout() {
		localStorage.removeItem("token");
		localStorage.removeItem("user");
	},

	getCurrentUser() {
		const user = localStorage.getItem("user");
		return user ? JSON.parse(user) : null;
	},

	getToken() {
		return localStorage.getItem("token");
	},

	isAuthenticated() {
		return !!this.getToken();
	},
};
