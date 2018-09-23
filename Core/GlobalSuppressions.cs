
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", 
	"CA1819:Properties should not return arrays", 
	Justification = "Classes in External-namespaces will be used as DTO so this doesn't matter", 
	Scope = "member",
	Target = "~P:Core.External.TrickResult.PlayedCards")]

