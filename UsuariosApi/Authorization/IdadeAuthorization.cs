using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace UsuariosApi.Authorization;

public class IdadeAuthorization : AuthorizationHandler<IdadeMinima>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IdadeMinima requirement)
    {
        // Busca a data de nascimento com base no token, percorrendo todos os claims e pegando o primeiro que for de data
        var dataNascimentoClaim = context.User.FindFirst(claim => claim.Type == ClaimTypes.DateOfBirth);
        
        if (dataNascimentoClaim is null) { return Task.CompletedTask; }
        
        var dataNascimento = Convert.ToDateTime(dataNascimentoClaim.Value);
        
        var idadeUsuario = DateTime.Today.Year - dataNascimento.Year;
        
        // Confere se a data de nascimento é maior que a data atual, subtraído a idade do usuário
        // caso sim, retira um ano, já que a pessoa ainda não fez aniversário
        if (dataNascimento > DateTime.Today.AddYears(-idadeUsuario)) { idadeUsuario--; }
        
        if (idadeUsuario >= requirement.Idade) { context.Succeed(requirement); }
        return Task.CompletedTask;
    }
}
