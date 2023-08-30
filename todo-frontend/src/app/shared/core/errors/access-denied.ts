export class AccessDeniedError extends Error {
  constructor() {
    super('Você não possui permissão para acessar esse recurso.');
    this.name = 'Acesso Negado';
  }
}
