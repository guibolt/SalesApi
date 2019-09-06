﻿using AutoMapper;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Util
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ClienteView, Cliente>();
            CreateMap<ProdutoView, Produto>();
            CreateMap<ProdutoView, Produto>();


            CreateMap<Cliente, Cliente>().ForMember(dest => dest.Id, opt => opt.Ignore())
             .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
             .ForMember(dest => dest.Genero, opt => opt.Condition(src => src.Genero != null))
             .ForMember(dest => dest.Nome, opt => opt.Condition(src => src.Nome != null))
           .ForMember(dest => dest.Cpf, opt => opt.Condition(src => src.Cpf != null))
             .ForMember(dest => dest.Idade, opt => opt.Condition(src => src.Idade!= 0));
              

            CreateMap<Produto, Produto>().ForMember(dest => dest.Id, opt => opt.Ignore())
             .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
             .ForMember(dest => dest.Nome, opt => opt.Condition(src => src.Nome != null))
            .ForMember(dest => dest.Preco, opt => opt.Condition(src => src.Preco != 0) )
            .ForMember(dest => dest.Quantidade, opt => opt.Condition(src => src.Quantidade!= 0))
            .ForMember(dest => dest.Categoria, opt => opt.Condition(src => src.Categoria != 0));


            CreateMap<Promocao, Promocao>().ForMember(dest => dest.Descricao, opt => opt.Condition(src => src.Descricao != null))
             .ForMember(dest => dest.Categoria, opt => opt.Condition(src => src.Categoria != 0))
             .ForMember(dest => dest.TaxaDesconto, opt => opt.Condition(src => src.TaxaDesconto != 0))
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
             .ForMember(dest => dest.DataFinal, opt => opt.Condition(src => src.DataFinal != null));






        }
    }
}
