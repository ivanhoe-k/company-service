import axios from './axios';

export interface TokenRequest {
  clientId: string;
  clientSecret: string;
}

export interface TokenResponse {
  token: string;
}

export const requestToken = async (credentials: TokenRequest): Promise<TokenResponse> => {
  const res = await axios.post('/auth/token', credentials);
  return res.data;
};
