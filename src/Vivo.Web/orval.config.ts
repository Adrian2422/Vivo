import { environment } from './src/environments/environment.development';

export default {
  vivo: {
    input: {
      target: [`${environment.apiUrl}swagger/v1/swagger.json`],
    },
    output: {
      mode: 'tags-split',
      baseUrl: environment.apiUrl,
      target: './src/app/shared/api/vivo.ts',
      schemas: './src/app/shared/api/model',
      client: 'angular',
      clean: true,
      mock: {
        generators: [
          {
            type: 'msw',
            baseUrl: environment.apiUrl,
          },
        ],
      },
      override: {
        angular: {
          retrievalClient: 'httpResource',
        },
      },
    },
    hooks: {
      afterAllFilesWrite:
        'prettier ./src/app/shared/api --write; eslint --fix "src/app/shared/api/**"',
    },
  },
};
