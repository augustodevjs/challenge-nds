export class NotFoundError extends Error {
  constructor() {
    super(
      'Não foi possível carregar os dados. Por favor, tente novamente. Se o problema persistir, contate o administrador do sistema',
    );
    this.name = 'Recurso Não Encontrado';
  }
}
