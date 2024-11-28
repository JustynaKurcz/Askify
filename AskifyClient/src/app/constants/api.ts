const API_URL = 'http://localhost:5244/api/v1';
export const API_CONSTANTS = {
  USERS: {
    BASE_PATH: `${API_URL}/users`,
    SIGN_IN: `${API_URL}/users/sign-in`,
    SIGN_UP: `${API_URL}/users/sign-up`,
  }
} as const;
