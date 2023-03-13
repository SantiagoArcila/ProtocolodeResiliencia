defmodule ResilenceProtocol do
  @priv_dir "#{:code.priv_dir(:resilence_protocol)}"

  @lib_dir @priv_dir <> "/lib"
  @graphs_dir @lib_dir <> "/graphs/"
  # will not work in windows
  @dot_binary "/usr/bin/dot"
  @graph_images_dir @lib_dir <> "/graphs/images/"

  defstruct ~w[data file path type uri]a

  @moduledoc """
  initial effor to simulate `ResilenceProtocol` simulation.
  """
  # @spec new()
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

  def to_dot_file(graph, graph_file \\ "temp.dot") do
    graphs_dir = @graphs_dir

    graph_data =
      Graph.to_dot(graph)
      |> case do
        {:ok, dot} -> dot
        {:error, error} -> raise error
      end

    File.write!(graphs_dir <> graph_file, graph_data)
  end

  def to_png(binary \\ :dot) do
    binary =
      case binary do
        :dot -> @dot_binary
      end

    dot_file = @graphs_dir <> "temp.dot"
    png_file = @graph_images_dir <> Path.basename(dot_file, ".dot") <> ".png"

    System.cmd(binary, ["-T", "png", dot_file, "-o", png_file])
  end
end
