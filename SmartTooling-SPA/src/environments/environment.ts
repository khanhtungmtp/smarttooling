const port: string = ':5000';
const protocol: string = window.location.protocol;
const hostname: string = window.location.hostname;
const host: string = `${hostname}${port}`;
const baseUrl: string = `${protocol}//${host}`;

export const environment = {
  production: false,
  apiUrl: `${baseUrl}/api/`,
  imageUrl: `${baseUrl}/uploaded/`,
  baseUrl: `${baseUrl}/`,
  allowedDomains: [host],
  disallowedRoutes: [`${host}/api/auth`]
};