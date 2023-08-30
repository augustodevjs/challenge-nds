export class UnexpectedError extends Error {
  constructor(message?: string) {
    super(
      message ??
        "Algo de errado aconteceu. Por favor, tente novamente. Se o problema persistir, contate o administrador do sistema."
    );
    this.name = "Erro Inesperado";
  }
}
