defmodule ResilenceProtocol do
  @priv_dir "#{:code.priv_dir(:resilence_protocol)}"

  @lib_dir @priv_dir <> "/lib"

  defstruct ~w[data file path type uri]a

  @moduledoc """
  initial effor to simulate `ResilenceProtocol` simulation.
  """

  def new(graph_data, graph_file, graph_type) do
    graphs_dir =
      case graph_type do
        :lib -> @lib_dir <> "/graphs/"
        _ -> raise "! Unknown graph_type: " <> graph_type
      end

    %__MODULE__{
      data: graph_data,
      file: graph_file,
      path: graphs_dir <> graph_file,
      type: graph_type,
      uri: "file://" <> graphs_dir <> graph_file
    }
  end
end
